public static int Execute(string command, string directory) => Node.Net.Diagnostics.ConsoleCommand.Execute(command, directory).ExitCode;

#region Assembly
public static Stream FindManifestResourceStream(this Assembly assembly, string pattern) => Node.Net.AssemblyExtension.GetStream(assembly, pattern);
#endregion

// DependencyObject
public static DependencyObject Clone(this DependencyObject d) => Node.Net.DependencyObjectExtension.Clone(d);

#region System.Windows.Point
public static double GetArea(this Point[] boundary) => Node.Net.PointExtension.GetArea(boundary);
public static Point GetPointAtDistance(this Point[] boundary, double distance) => Node.Net.PointExtension.GetPointAtDistance(boundary, distance);
public static Point[] Close(this Point[] points, double tolerance = 0.0001) => Node.Net.PointExtension.Close(points, tolerance);
public static Point[] Open(this Point[] points, double tolerance = 0.0001) => Node.Net.PointExtension.Open(points, tolerance);
public static double GetLength(this Point[] points) => Node.Net.PointExtension.GetLength(points);
public static Point[] GetPointsAtInterval(this Point[] points, double interval) => Node.Net.PointExtension.GetPointsAtInterval(points, interval);
public static double GetArea(this Point[] points) => Node.Net.PointExtension.GetArea(points);
public static Point GetCentroid(this Point[] points) => Node.Net.PointExtension.GetCentroid(points);
public static Point[] Offset(this Point[] points, double distance) => Node.Net.PointExtension.Offset(points, distance);
public static Point[] Scale(this Point[] points, double scale) => Node.Net.PointExtension.Scale(points, scale);
public static Point[] Reverse(this Point[] points) => Node.Net.PointExtension.Reverse(points);
public static Point[] ParsePoints(string value) => Node.Net.PointExtension.ParsePoints(value);
public static string GetString(this Point[] points) => Node.Net.PointExtension.GetString(points);
public static bool Contains(this Point[] points, Point point, double epsilon = 0.001) => Node.Net.PointExtension.Contains(points, point, epsilon);
#endregion System.Windows.Point


#region System.Collections.IDictionary
public static void Save(this IDictionary dictionary, Stream stream) => Node.Net.IDictionaryExtension.Save(dictionary, stream);
public static void Save(this IDictionary dictionary, string name) => Node.Net.IDictionaryExtension.Save(dictionary, name);
public static string GetJSON(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetJSON(dictionary);
public static IList Collect(this IDictionary dictionary, string type) => Node.Net.IDictionaryExtension.Collect(dictionary, type);
public static IList Collect(this IDictionary dictionary, Type type, string search) => Node.Net.IDictionaryExtension.Collect(dictionary, type, search);
public static IList<T> Collect<T>(this IDictionary dictionary, string search) => Node.Net.IDictionaryExtension.Collect<T>(dictionary, search);
public static IList<T> Collect<T>(this IDictionary dictionary) => Node.Net.IDictionaryExtension.Collect<T>(dictionary);
public static IList<T> Collect<T>(this IDictionary dictionary, KeyValuePair<string, string> kvp) where T : IDictionary => Node.Net.IDictionaryExtension.Collect<T>(dictionary, kvp);
public static T Find<T>(this IDictionary dictionary, string name, bool exact = false) => Node.Net.IDictionaryExtension.Find<T>(dictionary, name, exact);
public static string GetFileName(this IDictionary element) => Node.Net.ObjectExtension.GetFileName(element);
public static void SetFileName(this IDictionary element, string filename) => Node.Net.ObjectExtension.SetFileName(element, filename);
public static string GetTypeName(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetTypeName(dictionary);
public static void SetTypeName(this IDictionary dictionary, string typename) => Node.Net.IDictionaryExtension.Set(dictionary, "Type", typename);
public static string GetName(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetName(dictionary);
public static string GetFullName(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetFullName(dictionary);
public static double GetLengthMeters(this IDictionary dictionary, string name) => Node.Net.IDictionaryExtension.GetLengthMeters(dictionary, name);
public static double GetAngleDegrees(this IDictionary dictionary, string name) => Node.Net.IDictionaryExtension.GetAngleDegrees(dictionary, name);
public static object GetParent(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetParent(dictionary);
public static void SetParent(this IDictionary dictionary, object parent) => Node.Net.IDictionaryExtension.SetParent(dictionary, parent);
public static T Get<T>(this IDictionary dictionary, string name, T defaultValue = default(T), bool search = false) => Node.Net.IDictionaryExtension.Get<T>(dictionary, name, defaultValue, search);
public static void Set(this IDictionary dictionary, string key, object value) => Node.Net.IDictionaryExtension.Set(dictionary, key, value);
public static IDictionary GetAncestor(this IDictionary child, string key, string value) => Node.Net.IDictionaryExtension.GetAncestor(child, key, value);
public static T GetNearestAncestor<T>(this IDictionary child) => Node.Net.IDictionaryExtension.GetNearestAncestor<T>(child);
public static T GetFurthestAncestor<T>(this IDictionary child) => Node.Net.IDictionaryExtension.GetFurthestAncestor<T>(child);
public static IDictionary GetRootAncestor(this IDictionary child) => Node.Net.IDictionaryExtension.GetRootAncestor(child) as IDictionary;
public static Point3D GetWorldOrigin(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetWorldOrigin(dictionary);
public static Vector3D GetWorldRotations(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetWorldRotations(dictionary);
public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetLocalToParent(dictionary);
public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetLocalToWorld(dictionary);
public static Matrix3D GetWorldToLocal(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetWorldToLocal(dictionary);
public static Vector3D GetRotations(this IDictionary dictionary) => Node.Net.IDictionaryExtension.GetRotations(dictionary);
public static string GetUniqueKey(this IDictionary dictionary, string basename) => Node.Net.IDictionaryExtension.GetUniqueKey(dictionary, basename);
public static void Save(this IDictionary dictionary, Stream stream) => Node.Net.IDictionaryExtension.Save(dictionary, stream);
public static void Save(this IDictionary dictionary, string name) => Node.Net.IDictionaryExtension.Save(dictionary, name);
public static double GetLengthMeters(this IDictionary dictionary, string name) => Node.Net.IDictionaryExtension.GetLengthMeters(dictionary, name);
public static double GetAngleDegrees(this IDictionary dictionary, string name) => Node.Net.IDictionaryExtension.GetAngleDegrees(dictionary, name);
#endregion System.Collections.IDictionary

#region ImageSource
public static Material GetMaterial(this ImageSource imageSource) => Node.Net.ImageSourceExtension.GetMaterial(imageSource);
#endregion

#region Matrix3D
public static Matrix3D SetDirectionVectors(this Matrix3D matrix, Vector3D x, Vector3D y, Vector3D z) => Node.Net.Matrix3DExtension.SetDirectionVectors(matrix, x, y, z);
public static Vector3D GetRotationsXYZ(this Matrix3D matrix) => Node.Net.Matrix3DExtension.GetRotationsXYZ(matrix);
public static IDictionary GetDictionary(this Matrix3D matrix) => Node.Net.Matrix3DExtension.GetDictionary(matrix);
public static Matrix3D RotateXYZ(this Matrix3D matrix, Vector3D rotations) => Node.Net.Matrix3DExtension.RotateXYZ(matrix, rotations);

#endregion Matrix3D

#region System.Object
public static object GetParent(this object source) => Node.Net.ObjectExtension.GetParent(source);
public static void SetParent(this object source, object parent) => Node.Net.ObjectExtension.SetParent(source, parent);
var name = Node.Net.ObjectExtension.GetName(source);
public static object GetKey(this object source) => Node.Net.ObjectExtension.GetKey(source);
public static bool HasPropertyValue(this object item, string propertyName) => Node.Net.ObjectExtension.HasPropertyValue(item, propertyName);
public static T GetPropertyValue<T>(this object item, string propertyName, T defaultValue = default(T)) => Node.Net.ObjectExtension.GetPropertyValue<T>(item, propertyName, defaultValue);
public static void SetPropertyValue(this object item, string propertyName, object propertyValue) => Node.Net.ObjectExtension.SetPropertyValue(item, propertyName, propertyValue);

#endregion

public static Point3D[] ParsePoints(string value) => Node.Net.Point3DExtension.ParsePoints(value);
public static Point[] Get2DPoints(this Point3D[] points) => Node.Net.Point3DExtension.Get2DPoints(points);


public static bool IsVisible(this PerspectiveCamera camera, Point3D point) => Node.Net.PerspectiveCameraExtension.IsVisible(camera, point);

public static Rect3D Scale(this Rect3D rect, double factor) => Node.Net.Rect3DExtension.Scale(rect, factor);
public static void CopyToFile(this Stream stream, string filename) => Node.Net.StreamExtension.CopyToFile(stream, filename);
public static double GetMeters(this string instance) => Node.Net.StringExtension.GetMeters(instance);
public static Vector3D GetPerpendicular(this Vector3D vector) => Node.Net.Vector3DExtension.GetPerpendicular(vector);
private Node.Net.Factory factory = new Node.Net.Factory
private Node.Net.Reader reader = new Node.Net.Reader();
wordReader = new Node.Net.WordReader(streamReader);
private Node.Net.WordReader wordReader;
private Node.Net.Writer writer = new Node.Net.Writer();

