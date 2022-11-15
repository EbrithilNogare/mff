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
            Vec3f LoDirect = Vec3f(0);

            auto intersection = mScene.FindClosestIntersection(ray);
            if (intersection)
            {
                if (intersection->lightID >= 0) {
                    const AbstractLight* light = mScene.GetLightPtr(intersection->lightID);
                    mFramebuffer.AddColor(sample, Vec3f(light->Evaluate(ray.direction)));
                    continue;
                }

                const Vec3f surfacePoint = ray.origin + ray.direction * intersection->distance;
                CoordinateFrame frame;
                frame.SetFromZ(intersection->normal);
                const Vec3f incomingDirection = frame.ToLocal(-ray.direction);

                const Material& mat = mScene.GetMaterial(intersection->materialID);

                auto newDir = mRandomGenerator.GetRandomOnHemiSphere(intersection->normal);
                Ray ray2 = Ray(surfacePoint, newDir, EPSILON_RAY);

                auto intersection2 = mScene.FindClosestIntersection(ray2);
                if (intersection2){
                    if (intersection2->lightID >= 0) {
                        const AbstractLight* light = mScene.GetLightPtr(intersection2->lightID);

                        float cosTheta = Dot(frame.mZ, newDir);
                        auto intensity = light->Evaluate(ray2.direction);
                        if (cosTheta > 0 && intensity.Max() > 0)
                        {
                            LoDirect += intensity * mat.EvaluateBRDF(frame.ToLocal(newDir), incomingDirection) * cosTheta;
                        }
                    }
                    mFramebuffer.AddColor(sample, LoDirect);
                }
                else {
                    auto background = mScene.GetBackground();
                    if (!background)continue;
                    float cosTheta = Dot(frame.mZ, newDir);
                    auto intensity = background->Evaluate(ray2.direction);
                    if (cosTheta > 0 && intensity.Max() > 0)
                    {
                        LoDirect += intensity * mat.EvaluateBRDF(frame.ToLocal(newDir), incomingDirection) * cosTheta;
                    }
                }
                mFramebuffer.AddColor(sample, LoDirect);
            }
        }

        mIterations++;
    }

    Rng mRandomGenerator;
};
