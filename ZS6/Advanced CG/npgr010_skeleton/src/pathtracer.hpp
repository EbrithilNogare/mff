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
    bool solidLightInScene = false;
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

        for (int i = 0; i < mScene.GetLightCount(); i++)
        {
            const AbstractLight* light = mScene.GetLightPtr(i);
            if (light->hasVolume())
                solidLightInScene = true;
        }

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

            Vec3f color = estimateLin(ray);

            if (!std::isnan(color.x) && !std::isnan(color.y) && !std::isnan(color.z)) {
                mFramebuffer.AddColor(sample, color);
            }
            
        }

        mIterations++;
    }

    Vec3f estimateLin(Ray firstRay) {
        Vec3f throughput = Vec3f(1);
        Vec3f acum = Vec3f(0);
        Ray ray = firstRay;

        for(;;)
        {
            auto intersection = mScene.FindClosestIntersection(ray);

            // out of scene
            if (!intersection) {
                auto background = mScene.GetBackground();
                if (background) {
                    acum += throughput * background->Evaluate(ray.direction);
                }
                break;
            }

            // direct hit of the light
            if (intersection->lightID >= 0) {
                const AbstractLight* light = mScene.GetLightPtr(intersection->lightID);
                acum += throughput * Vec3f(light->Evaluate(ray.direction));
                break;
            }

            // hit of something
            const Vec3f surfacePoint = ray.origin + ray.direction * intersection->distance;
            CoordinateFrame frame;
            frame.SetFromZ(intersection->normal);
            const Vec3f incomingDirection = frame.ToLocal(-ray.direction);
            const Material& mat = mScene.GetMaterial(intersection->materialID);

            /// Sample lights
            Vec3f colorFromLights = SampleLights(surfacePoint, frame, mat, incomingDirection);
            throughput /= 2; // todo
            acum += throughput * colorFromLights;

            /// Sample BRDF
            Vec3f newDir;
            float diffuseProbability = mat.mDiffuseReflectance.Max();
            float specularProbability = mat.mPhongReflectance.Max();
            float normalization = 1.f / (diffuseProbability + specularProbability);
            diffuseProbability *= normalization;
            specularProbability *= normalization;
            
            Vec3f diffuseDirection = mRandomGenerator.GetRandomOnHemiSphere();
            float diffusePDF = diffuseProbability * mRandomGenerator.CosineHemispherePdf(diffuseDirection.z);
            Vec3f specularDirection = mRandomGenerator.rndHemiCosN(mat.mPhongExponent);
            float specularPDF = specularProbability * mRandomGenerator.rndHemiCosNPDF(specularDirection, mat.mPhongExponent);
            float pdf = 0;

            if (mRandomGenerator.GetFloat() <= diffuseProbability) { // diffuse
                CoordinateFrame lobeFrame;
                lobeFrame.SetFromZ(intersection->normal);
                newDir = lobeFrame.ToWorld(diffuseDirection);
                pdf = diffusePDF;
            }
            else { // specular
                CoordinateFrame lobeFrame = CoordinateFrame();
                Vec3f reflectedDir = ray.direction - 2 * (Dot(ray.direction, intersection->normal)) * intersection->normal;
                lobeFrame.SetFromZ(reflectedDir);
                newDir = lobeFrame.ToWorld(specularDirection);
                pdf = specularPDF;
            }

            if (std::isnan(pdf) || pdf == 0) {
                break;
            }


            ray = Ray(surfacePoint, newDir, EPSILON_RAY);
            float cosTheta = Dot(frame.mZ, newDir);
            throughput *= mat.EvaluateBRDF(frame.ToLocal(newDir), incomingDirection) * cosTheta / pdf;

            /*
            auto intersection2 = mScene.FindClosestIntersection(ray);
            float cosTheta = Dot(frame.mZ, newDir);
            if (intersection2) {
                if (intersection2->lightID >= 0) {
                    const AbstractLight* light = mScene.GetLightPtr(intersection2->lightID);
                    acum += throughput * Vec3f(light->Evaluate(ray.direction));
                    
                    break;
                }
            }
            else {
                auto background = mScene.GetBackground();
                if (background) {
                    auto intensity = background->Evaluate(ray.direction);
                    if (cosTheta > 0 && intensity.Max() > 0)
                    {
                        LoDirect += intensity * mat.EvaluateBRDF(frame.ToLocal(newDir), incomingDirection) * cosTheta / pdf;
                    }
                }
            }
            */


            float survivalProb = std::min(1.0f, throughput.Max());
            throughput /= survivalProb;
            if (mRandomGenerator.GetFloat() >= survivalProb) {
                break;
            }
        }

        return acum/2;
    }

    Vec3f SampleLights(const Vec3f& surfacePoint, CoordinateFrame& frame, const Material& mat, const Vec3f& incomingDirection)
    {
        Vec3f LoDirect = (0);
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
        return LoDirect;
    }

    Rng mRandomGenerator;
};
