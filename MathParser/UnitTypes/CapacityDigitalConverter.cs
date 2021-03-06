﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum CapacityDigitalUnits
    {
        [UnitPlural("Bits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Bit,
        [UnitPlural("Bytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Byte,
        [UnitAbbreviation("kb")]
        [UnitPlural("Kilobytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Kilobyte,
        [UnitAbbreviation("mb")]
        [UnitPlural("Megabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Megabyte,
        [UnitAbbreviation("gb")]
        [UnitPlural("Gigabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Gigabyte,
        [UnitAbbreviation("tb")]
        [UnitPlural("Terabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Terabyte,
        [UnitAbbreviation("pb")]
        [UnitPlural("Petabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Petabyte,
        [UnitAbbreviation("eb")]
        [UnitPlural("Exabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Exabyte,
        [UnitAbbreviation("zb")]
        [UnitPlural("Zettabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Zettabyte,
        [UnitAbbreviation("yb")]
        [UnitPlural("Yottabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Yottabyte,

        // Bits
        [UnitAbbreviation("kbit")]
        [UnitPlural("Kilobits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Kilobit,
        [UnitAbbreviation("mbit")]
        [UnitPlural("Megabits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Megabit,
        [UnitAbbreviation("gbit")]
        [UnitPlural("Gigabits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Gigabit,
        [UnitAbbreviation("tbit")]
        [UnitPlural("Terabits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Terabit,
        [UnitAbbreviation("pbit")]
        [UnitPlural("Petabits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Petabit,
        [UnitAbbreviation("ebit")]
        [UnitPlural("Exabits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Exabit,
        [UnitAbbreviation("zbit")]
        [UnitPlural("Zettabits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Zettabit,
        [UnitAbbreviation("ybit")]
        [UnitPlural("Yottabits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Yottabit,

        Unknown
    }
    public static class DigitalHlper
    {
        public static bool IsBit(this CapacityDigitalUnits unit)
        {
            return unit == CapacityDigitalUnits.Bit;// || (unit >= CapacityDigitalUnits.Kilobit && unit <= CapacityDigitalUnits.Yottabit);
        }
    }

    public class CapacityDigitalConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return CapacityDigitalUnits.Byte; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        private Dictionary<CapacityDigitalUnits, decimal> conversionMap = new Dictionary<CapacityDigitalUnits, decimal>();

        public CapacityDigitalConverter()
        {
            conversionMap.Add(CapacityDigitalUnits.Bit, 1);
            conversionMap.Add(CapacityDigitalUnits.Byte, 1);
            conversionMap.Add(CapacityDigitalUnits.Kilobyte, 1024);
            conversionMap.Add(CapacityDigitalUnits.Megabyte, 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Gigabyte, (long)1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Terabyte, (long)1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Petabyte, (long)1024 * 1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Exabyte, (decimal)1024 * 1024 * 1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Zettabyte, (decimal)1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Yottabyte, (decimal)1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024);

            conversionMap.Add(CapacityDigitalUnits.Kilobit, 125);
            conversionMap.Add(CapacityDigitalUnits.Megabit, 125 * 1000);
            conversionMap.Add(CapacityDigitalUnits.Gigabit, (long)125 * 1000 * 1000);
            conversionMap.Add(CapacityDigitalUnits.Terabit, (long)125 * 1000 * 1000 * 1000);
            conversionMap.Add(CapacityDigitalUnits.Petabit, (decimal)125 * 1000 * 1000 * 1000 * 1000);
            conversionMap.Add(CapacityDigitalUnits.Exabit, (decimal)125 * 1000 * 1000 * 1000 * 1000 * 1000);
            conversionMap.Add(CapacityDigitalUnits.Zettabit, (decimal)125 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000);
            conversionMap.Add(CapacityDigitalUnits.Yottabit, (decimal)125 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000);
        }

        /// <summary>
        /// Converts from 1 distance to another. By default, converts to Meter and then back to the other measurements
        /// </summary>
        /// <param name="value">The value of the from unit</param>
        /// <param name="from">From unit can be anything listed in DistanceUnits</param>
        /// <param name="to">To unit can be anything listed in DistanceUnits</param>
        /// <returns></returns>
        public override decimal Convert(decimal value, Enum from, Enum to)
        {
            CapacityDigitalUnits fromDu = (CapacityDigitalUnits)from;
            CapacityDigitalUnits toDu = (CapacityDigitalUnits)to;

            decimal conversion;

            if (!conversionMap.TryGetValue(fromDu, out conversion))
            {
                throw new InvalidUnitTypeException(fromDu.ToString());
            }
            // Convert from to Meter
            value *= conversion;

            // Convert meter to to.
            if (!toDu.IsBit() && !conversionMap.TryGetValue(toDu, out conversion))
            {
                throw new InvalidUnitTypeException(toDu.ToString());
            }

            // Handle converting to/from bits
            if (fromDu.IsBit() && !toDu.IsBit())
            {
                conversion *= 8;
            }
            else if (!fromDu.IsBit() && toDu.IsBit())
            {
                conversion *= (decimal)1 / 8;
            }

            return value / conversion;
        }

    }
}
