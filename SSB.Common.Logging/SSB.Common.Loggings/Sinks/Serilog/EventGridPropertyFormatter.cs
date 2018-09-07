using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Debugging;
using Serilog.Events;

namespace Ssb.Common.Logging.Sinks.Serilog
{
    /// <summary>
    /// This class is taken directly from the open source work of Chris Kirby
    /// and his serilog-sinks-eventgrid project: https://github.com/sirkirby/serilog-sinks-eventgrid
    /// This custom formatter uses recursion to reduce the standard Serilog LogEvent properties to 
    /// a compact key/value json compilation.
    /// </summary>
    public static class EventGridPropertyFormatter
    {
        private static readonly HashSet<Type> EventGridScalars = new HashSet<Type>
        {
            typeof(bool),typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal),typeof(byte[]),
            typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal),typeof(byte[])
        };

        public static object Simplify(LogEventPropertyValue value)
        {
            if (value.GetType() == typeof(ScalarValue))
            {
                var scalar = (ScalarValue)value;
                return SimplifyScalar(scalar.Value);
            }

            if (value.GetType() == typeof(DictionaryValue))
            {
                var dict = (DictionaryValue)value;
                var result = new Dictionary<object, object>();

                foreach (var element in dict.Elements)
                {
                    var key = SimplifyScalar(element.Key.Value);

                    if (result.ContainsKey(key))
                    {
                        SelfLog.WriteLine("The key {0} is not unique in the provided dictionary after simplification to {1}.", element.Key, key);

                        return dict.Elements.Select(e => new Dictionary<string, object>
                        {
                            { "Key", SimplifyScalar(e.Key.Value) },
                            { "Value", Simplify(e.Value) }
                        }).ToArray();
                    }

                    result.Add(key, Simplify(element.Value));
                }

                return result;
            }

            if (value.GetType() == typeof(SequenceValue))
            {
                var seq = (SequenceValue)value;
                return seq.Elements.Select(Simplify).ToArray();
            }

            if (value.GetType() != typeof(StructureValue))
                return null;

            var str = (StructureValue)value;

            var props = str.Properties.ToDictionary(p => p.Name, p => Simplify(p.Value));

            if (str.TypeTag != null)
                props["$typeTag"] = str.TypeTag;

            return props;
        }

        private static object SimplifyScalar(object value)
        {
            if (value == null)
                return null;

            var valueType = value.GetType();

            return EventGridScalars.Contains(valueType) ? value : value.ToString();
        }
    }
}
