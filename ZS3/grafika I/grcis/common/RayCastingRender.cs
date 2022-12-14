using System;
using System.Collections.Generic;
using OpenTK;

namespace Rendering
{
  /// <summary>
  /// Ray-casting rendering (ray-tracing w/o all secondary rays).
  /// </summary>
  [Serializable]
  public class RayCasting : IImageFunction
  {
    /// <summary>
    /// Hash-multiplier for number of light sources.
    /// </summary>
    protected const long HASH_LIGHT = 101L;

    /// <summary>
    /// Hash-multiplier for textures.
    /// </summary>
    protected const long HASH_TEXTURE = 11L;

    /// <summary>
    /// Domain width.
    /// </summary>
    public double Width
    {
      get => scene.Camera.Width;
      set => scene.Camera.Width = value;
    }

    /// <summary>
    /// Domain height.
    /// </summary>
    public double Height
    {
      get => scene.Camera.Height;
      set => scene.Camera.Height = value;
    }

    protected IRayScene scene;

    public RayCasting (IRayScene sc)
    {
      scene = sc;
    }

    /// <summary>
    /// Computes one image sample. Internal integration support.
    /// </summary>
    /// <param name="x">Horizontal coordinate.</param>
    /// <param name="y">Vertical coordinate.</param>
    /// <param name="color">Computed sample color.</param>
    /// <returns>Hash-value used for adaptive subsampling.</returns>
    public virtual long GetSample (double x, double y, double[] color)
    {
      Vector3d p0, p1;
      int      bands = color.Length;
      if (!scene.Camera.GetRay(x, y, out p0, out p1))
      {
        Array.Clear(color, 0, bands); // invalid ray -> black color
        return 1L;
      }

      LinkedList<Intersection> intersections = scene.Intersectable.Intersect(p0, p1);
      Intersection.countRays++;
      Intersection i = Intersection.FirstIntersection(intersections, ref p1);
      if (i == null) // no intersection -> background color
      {
        Array.Copy(scene.BackgroundColor, color, bands);
        return 0L;
      }

      // there was at least one intersection
      i.Complete();

      // hash code for adaptive supersampling:
      long hash = i.Solid.GetHashCode();

      // apply all the textures fist..
      if (i.Textures != null)
        foreach (ITexture tex in i.Textures)
          hash = hash * HASH_TEXTURE + tex.Apply(i);

      // terminate if light sources are missing
      if (scene.Sources == null || scene.Sources.Count < 1)
      {
        Array.Copy(i.SurfaceColor, color, bands);
        return hash;
      }

      // .. else apply the reflectance model for each source
      p1 = -p1;
      p1.Normalize();

      i.Material = (IMaterial)i.Material.Clone();
      i.Material.Color = i.SurfaceColor;
      Array.Clear(color, 0, bands);

      foreach (ILightSource source in scene.Sources)
      {
        Vector3d dir;
        double[] intensity = source.GetIntensity(i, out dir);
        if (intensity != null)
        {
          double[] reflection = i.ReflectanceModel.ColorReflection(i, dir, p1, ReflectionComponent.ALL);
          if (reflection != null)
          {
            for (int b = 0; b < bands; b++)
              color[b] += intensity[b] * reflection[b];
            hash = hash * HASH_LIGHT + source.GetHashCode();
          }
        }
      }

      return hash;
    }
  }
}
