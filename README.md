# DateTag
DateTag is a class for representing a Date metadata tag. When a date metadata is represented in [W3CDTF](https://www.w3.org/TR/NOTE-datetime) format there are three attributes that can be extracted from the field:

* The DateTime being represented.
* The TimeZone information.
* The Precision of the DateTime.

This C# class represents all three components and includes for parsing, formatting, and using DateTime values. Even when DateTime data are not represented in W3CDTF format, the class is valuable for combining and representing all three related attributes.

## DateTime

The DateTime contains both the date and the time of the event associated with the metadata. The value is usually, but not always, in local time. In some cases, the DateTime may be in UTC (see TimeZone). Here are examples of the DateTime in W3CDTF format:

* 2019-01-23
* 2019-01-23T12:45
* 2019-01-23T12:50:22.213

## TimeZone

The TimeZone information is included using a [TimeZoneTag](https://github.com/FileMeta/TimeZoneTag) instance. Under ideal conditions, the TimeZone value contains the timezone in which the event occurred. This allows a local DateTime to be converted to UTC for indexing and sorting purposes. However, some file formats don't include timezone information and some devices, such as cameras, may not have time zone information. So, there are two special values of TimeZoneTag.

ForceUTC indicates that the value should be interpreted as UTC regardless of the file format specification. In W3CDTF format this is indicated by a "Z" timezone suffix. As a standalone timezone value it is represented by "Z".

ForceLocal indicates that the value should be interpreted as Local time regardless of the file format definition. In W3CDTF format, ForceLocal is indicated by the absence of any timezone suffix. As a standalone value it is represented by "0".

ForceLocal is used on video files produced by certain digital cameras. The MP4 video file format specification indicates that DateTime should be in UTC. However, many digital cameras don't have a timezone setting. In these cases, the digital camera puts local time into the field even though it's defined as UTC.

Here are examples of W3CDTF format times with various timezone suffix values.

<table>
<tr><th>Sample</th><th>Timezone Interpretation</th></tr>
<tr><td>2019-01-23T13:30-05:00</td><td>Local time UTC minus five hours (U.S. Eastern Time).</td></tr>
<tr><td>2019-01-23T13:30+01:00</td><td>Local time UTC plus one hour (Central European Time).</td></tr>
<tr><td>2019-01-23T13:30Z</td><td>UTC time (ForceUTC). Offset from UTC unspecified.</td></tr>
<tr><td>2019-01-23T13:30</td><td>Local time (ForceLocal) Offset from UTC unspecified.</td></tr>
</table>

## Precision

Date values in W3CDTF format may be truncated according to the precision of available information. Too often, this precision information is lost when the field is parsed. The DateTag class preserves precision as the number of significant digits.

Here are examples of Date values in W3CDTF format including their precision.

<table>
<tr><th>Sample</th><th>Precision</th><th>Description</th></tr>
<tr><td>2019</td><td>4</td><td>Year</td></tr>
<tr><td>2019-01</td><td>6</td><td>Month</td></tr>
<tr><td>2019-01-23</td><td>8</td><td>Day</td></tr>
<tr><td>2019-01-23T13</td><td>10</td><td>Hour</td></tr>
<tr><td>2019-01-23T13:30</td><td>12</td><td>Minute</td></tr>
<tr><td>2019-01-23T13:30:21</td><td>14</td><td>Second</td></tr>
<tr><td>2019-01-23T13:30:21.482</td><td>17</td><td>Millisecond</td></tr>
<tr><td>2019-01-23T13:30:21:482496</td><td>20</td><td>Microsecond</td></tr>
</table>

## About DateTag
The software is distributed in C# as a [CodeBit](http://filemeta.org/CodeBit.html) located [here](https://raw.githubusercontent.com/FileMeta/DateTag/master/DateTag.cs). It is released under a [BSD 3-Clause](https://opensource.org/licenses/BSD-3-Clause) open source license.

DateTag is part of the [FileMeta](http://www.filemeta.org/) initiative because it manages a particular type of metadata field.

This project includes the master copy of the [DateTag.cs]((https://raw.githubusercontent.com/FileMeta/DateTag/master/DateTag.cs) CodeBit plus a set of unit tests which may also serve as sample code.

DateTag depends on the [TimeZoneTag](https://github.com/FileMeta/TimeZoneTag) CodeBit.

## About CodeBits
A [CodeBit](https://www.FileMeta.org/CodeBit.html) is a way to share common code that's lighter weight than NuGet. Each CodeBit consists of a single source code file. A structured comment at the beginning of the file indicates where to find the master copy so that automated tools can retrieve and update CodeBits to the latest version.
