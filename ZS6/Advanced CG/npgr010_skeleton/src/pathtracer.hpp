#pragma once

#include <vector>
#include <cmath>
#include <omp.h>
#include <cassert>
#include "renderer.hpp"
#include "rng.hpp"

class PathTracer : public AbstractRenderer
{
public:

    PathTracer(
        const Scene& aScene,
        int aSeed = 1234
    ) :
        AbstractRenderer(aScene), mRandomGenerator(aSeed)
    {}

    virtual void RunIteration(int iteration)
    {
        const int resolutionX = int(mScene.mCamera.mResolution.x);
        const int resolutionY = int(mScene.mCamera.mResolution.y);

        for (int pixelID = 0; pixelID < resolutionX * resolutionY; pixelID++)
        {
            // Current pixel coordinates (as integers):
            const int x = pixelID % resolutionX;
            const int y = pixelID / resolutionX;

            // Current pixel coordinates (as floating point numbers, randomly positioned inside a pixel square):
            // E.g., for x = 5, y = 12, we can have sample coordinates from x = 5.00 to 5.99.., and y = 12.00 to 12.99..
            const Vec2f sample = Vec2f(float(x), float(y)) + mRandomGenerator.GetVec2f();

            // Generating a ray with an origin in the camera with a direction corresponding to the pixel coordinates:
            Ray ray = mScene.mCamera.GenerateRay(sample);

            auto intersection = mScene.FindClosestIntersection(ray);
            if (intersection)
            {
				const Vec3f surfacePoint = ray.origin + ray.direction * intersection->distance;
				CoordinateFrame frame;
				frame.SetFromZ(intersection->normal);
				const Vec3f incomingDirection = frame.ToLocal(-ray.direction);

				Vec3f LoDirect = Vec3f(0);
				const Material& mat = mScene.GetMaterial(intersection->materialID);

                // Connect from the current surface point to every light source in the scene:
				for (int i=0; i<mScene.GetLightCount(); i++)
				{
					const AbstractLight* light = mScene.GetLightPtr(i);
					assert(light != 0);

                    auto [lightPoint, intensity, pdf] = light->SamplePointOnLight(surfacePoint, mRandomGenerator);
                    Vec3f outgoingDirection = Normalize(lightPoint - surfacePoint);
                    float lightDistance = sqrt((lightPoint - surfacePoint).LenSqr());
                    float cosTheta = Dot(frame.mZ, outgoingDirection);
					
					if (cosTheta > 0 && intensity.Max() > 0)
					{
                        Ray rayToLight(surfacePoint, outgoingDirection, EPSILON_RAY); // Note! To prevent intersecting the same object we are already on, we need to offset the ray by EPSILON_RAY
						if (!mScene.FindAnyIntersection(rayToLight, lightDistance)) { // Testing if the direction towards the light source is not occluded
							LoDirect += intensity * mat.EvaluateBRDF(frame.ToLocal(outgoingDirection), incomingDirection) * cosTheta / pdf;
                        }
					}
				}

				mFramebuffer.AddColor(sample, LoDirect);
            }
        }

        mIterations++;
    }

    Rng mRandomGenerator;
};
