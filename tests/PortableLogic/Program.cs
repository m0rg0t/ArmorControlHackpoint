using System;
using System.Collections.Generic;
using System.Globalization;
using BluetoothClientWP8.Model;
class Program
{
    static int checks, failures;
    static void Check(bool ok, string label) { checks++; if (!ok) { failures++; Console.WriteLine("FAIL: " + label); } }
    static void Main()
    {
        var previous = CultureInfo.CurrentCulture;
        try {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            var item = new AccelerationItem();
            var names = new List<string>();
            item.PropertyChanged += (sender, args) => names.Add(args.PropertyName);
            item.Message = "1.25 -2.5 3.75";
            Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "valid telemetry");
            Check(names.Contains("X") && names.Contains("Y") && names.Contains("Z") && names.Contains("Message"), "valid notifications");
            names.Clear(); item.Message = "9 broken 8";
            Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "malformed frame preserves entire last sample");
            Check(names.Count == 1 && names[0] == "Message", "malformed frame has no partial axis notifications");
            item.Message = null;
            Check(item.X == 1.25 && item.Message == null, "null frame preserves sample and raw message");
            item.Message = ""; Check(item.Z == 3.75, "empty frame preserves sample");
            item.Message = "9 8"; Check(item.X == 1.25 && item.Y == -2.5, "short frame preserves sample");
            item.Message = "4.5 5.5 6.5"; Check(item.X == 4.5 && item.Y == 5.5 && item.Z == 6.5, "subsequent valid frame recovers");
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            item.Message = "1,25 -2,5 3,75"; Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "existing current-culture numeric contract retained");
            item.Message = "7 8 broken"; Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "invalid final axis preserves entire previous sample");
            foreach (string culture in new [] { "en-US", "ru-RU" }) {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                foreach (string invalid in new [] { Double.NaN.ToString(), Double.PositiveInfinity.ToString(), Double.NegativeInfinity.ToString(), "+Infinity", "-Infinity", "1e999", "-1e999" }) {
                    for (int axis = 0; axis < 3; axis++) {
                        item.Message = "1 2 3";
                        names.Clear();
                        var fields = new [] { "7", "8", "9" };
                        fields[axis] = invalid;
                        string frame = String.Join(" ", fields);
                        item.Message = frame;
                        string label = culture + " axis " + axis + " " + invalid;
                        Check(item.X == 1 && item.Y == 2 && item.Z == 3, "nonfinite frame preserves all axes: " + label);
                        Check(names.Count == 1 && names[0] == "Message", "nonfinite frame has no axis notifications: " + label);
                        Check(item.Message == frame, "raw nonfinite frame remains inspectable: " + label);
                    }
                }
                item.Message = "1e308 -1e308 0";
                Check(item.X == 1e308 && item.Y == -1e308 && item.Z == 0, "finite exponent extremes remain valid: " + culture);
            }
            var hit = new HitItem(); string notified = null;
            hit.PropertyChanged += (sender, args) => notified = args.PropertyName;
            hit.HitValue = 7; Check(hit.HitValue == 7 && notified == "HitValue", "hit value notifies its actual property");
        } finally { CultureInfo.CurrentCulture = previous; }
        Console.WriteLine(checks + " checks; " + failures + " failures");
        Environment.ExitCode = failures == 0 ? 0 : 1;
    }
}
