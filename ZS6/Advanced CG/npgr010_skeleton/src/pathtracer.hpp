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

                /// Sample lights

                for (int i = 0; i < mScene.GetLightCount(); i++)
                {
                    const AbstractLight* light = mScene.GetLightPtr(i);
                    assert(light != 0);

                    auto [lightPoint, intensity, pdf] = light->SamplePointOnLight(surfacePoint, mRandomGenerator);
                    Vec3f outgoingDirection = Normalize(lightPoint - surfacePoint);
                    float lightDistance = sqrt((lightPoint - surfacePoint).LenSqr());
                    float cosTheta = Dot(frame.mZ, outgoingDirection);

                    if (cosTheta > 0 && intensity.Max() > 0)
                    {
                        Ray rayToLight(surfacePoint, outgoingDirection, EPSILON_RAY);
                        if (!mScene.FindAnyIntersection(rayToLight, lightDistance)) {
                            LoDirect += intensity * mat.EvaluateBRDF(frame.ToLocal(outgoingDirection), incomingDirection) * cosTheta / pdf;
                        }
                    }
                }

                /// Sample BRDF

                Vec3f newDir;
                float pdf;

                float probDiffuse = mat.mDiffuseReflectance.Max();
                float probSpecular = mat.mPhongReflectance.Max();
                float normalization = 1.f / (probDiffuse + probSpecular);
                probDiffuse *= normalization;
                probSpecular *= normalization;

                float uniformRand = mRandomGenerator.GetFloat();

                Vec3f diffuseDirection = mRandomGenerator.GetRandomOnHemiSphere();
                Vec3f specularDirection = mRandomGenerator.rndHemiCosN(mat.mPhongExponent);

                if (uniformRand <= probDiffuse) { // diffuse
                    CoordinateFrame lobeFrame;
                    lobeFrame.SetFromZ(intersection->normal);
                    newDir = lobeFrame.ToWorld(diffuseDirection);
                    pdf = probDiffuse * mRandomGenerator.CosineHemispherePdf(diffuseDirection.z);
                }
                else { // specular
                    CoordinateFrame lobeFrame = CoordinateFrame();
                    Vec3f reflectedDir = ray.direction - 2 * (Dot(ray.direction, intersection->normal)) * intersection->normal;
                    lobeFrame.SetFromZ(reflectedDir);
                    newDir = lobeFrame.ToWorld(specularDirection);
                    pdf = probSpecular * mRandomGenerator.rndHemiCosNPDF(reflectedDir, mat.mPhongExponent);
                }
                
                Ray ray2 = Ray(surfacePoint, newDir, EPSILON_RAY);

                auto intersection2 = mScene.FindClosestIntersection(ray2);
                if (intersection2){
                    if (intersection2->lightID >= 0) {
                        const AbstractLight* light = mScene.GetLightPtr(intersection2->lightID);

                        float cosTheta = Dot(frame.mZ, newDir);
                        auto intensity = light->Evaluate(ray2.direction);
                        if (cosTheta > 0 && intensity.Max() > 0)
                        {
                            LoDirect += intensity * mat.EvaluateBRDF(frame.ToLocal(newDir), incomingDirection) * cosTheta / pdf;
                        }
                    }
                }
                else {
                    auto background = mScene.GetBackground();
                    if (background){
                        float cosTheta = Dot(frame.mZ, newDir);
                        auto intensity = background->Evaluate(ray2.direction);
                        if (cosTheta > 0 && intensity.Max() > 0)
                        {
                            LoDirect += intensity * mat.EvaluateBRDF(frame.ToLocal(newDir), incomingDirection) * cosTheta / pdf;
                        }
                    }
                }
                mFramebuffer.AddColor(sample, LoDirect/2);
            }
        }

        mIterations++;
    }

    Rng mRandomGenerator;
};
