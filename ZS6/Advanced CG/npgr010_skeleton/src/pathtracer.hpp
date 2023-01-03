#pragma once

#include <vector>
#include <cmath>
#include <omp.h>
#include <cassert>
#include "renderer.hpp"
#include "rng.hpp"

    class PathTracer : public AbstractRenderer
{
private:
    Rng mRandomGenerator;

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

        if (mScene.GetLightCount() == 0)
            return;

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

            if (std::isfinite(color.x) && std::isfinite(color.y) && std::isfinite(color.z)) {
                mFramebuffer.AddColor(sample, color);
            }
            else {
                mFramebuffer.AddColor(sample, Vec3f(1000,0,0));
            }
        }

        mIterations++;
    }

    Vec3f estimateLin(Ray firstRay) {
        Vec3f throughput = Vec3f(1);
        Vec3f acum = Vec3f(0);
        Ray ray = firstRay;


        for (int bounces = 0;; bounces++)
        {
            std::optional<Intersection> intersection = mScene.FindClosestIntersection(ray);

            /// out of scene
            if (!intersection) {
                auto background = mScene.GetBackground();
                if (background) {
                    acum += throughput * background->Evaluate(ray.direction);
                }
                break;
            }


            /// direct hit of the light
            if (intersection->lightID >= 0) {
                if (bounces == 0) {
                    const AbstractLight* light = mScene.GetLightPtr(intersection->lightID);
                    acum += throughput * Vec3f(light->Evaluate(ray.direction));
                }
                break;
            }


            /// hit of something
            const Vec3f surfacePoint = ray.origin + ray.direction * intersection->distance;
            CoordinateFrame hitLobeFrame;
            hitLobeFrame.SetFromZ(intersection->normal);
            const Vec3f incomingDirection = hitLobeFrame.ToLocal(-ray.direction);
            const Material& mat = mScene.GetMaterial(intersection->materialID);


            /// Sample lights
            Vec3f colorFromLights = 0;
            int randomLightIndex = mRandomGenerator.GetInt() % mScene.GetLightCount();
            const AbstractLight* light = mScene.GetLightPtr(randomLightIndex);
            auto [lightPoint, intensity, lightPDF] = light->SamplePointOnLight(surfacePoint, mRandomGenerator);
            //lightPDF /= mScene.GetLightCount();
            Vec3f lightDirection = Normalize(lightPoint - surfacePoint);
            float lightDistance = (lightPoint - surfacePoint).Length();

            Ray rayToLight(surfacePoint, lightDirection, EPSILON_RAY);
            if (!mScene.FindAnyIntersection(rayToLight, lightDistance)) {
                colorFromLights = intensity * mat.EvaluateBRDF(hitLobeFrame.ToLocal(lightDirection), incomingDirection) * std::max(0.0f, Dot(hitLobeFrame.mZ, lightDirection)) / lightPDF;
            }


            /// Sample BRDF
            // probabilities
            float diffuseProbability = mat.mDiffuseReflectance.Max();
            float specularProbability = mat.mPhongReflectance.Max();
            float normalization = 1.f / (diffuseProbability + specularProbability);
            diffuseProbability *= normalization;
            specularProbability *= normalization;

            // diffuse direction
            CoordinateFrame intersectionNormalLobe;
            intersectionNormalLobe.SetFromZ(intersection->normal);
            Vec3f diffuseRandomDirection = mRandomGenerator.CosineSampleHemisphere();
            Vec3f diffuseDirection = intersectionNormalLobe.ToWorld(diffuseRandomDirection);

            // specular direction
            Vec3f reflectedDir = ray.direction - 2 * (Dot(ray.direction, intersection->normal)) * intersection->normal;
            CoordinateFrame reflectedLobeFrame;
            reflectedLobeFrame.SetFromZ(reflectedDir);
            Vec3f specularRandomDirection = mRandomGenerator.rndHemiCosN(mat.mPhongExponent);
            Vec3f specularDirection = reflectedLobeFrame.ToWorld(specularRandomDirection);

            // brdf direction
            Vec3f brdfDirection;
            if (mRandomGenerator.GetFloat() <= diffuseProbability) {
                brdfDirection = diffuseDirection;
            }
            else {
                brdfDirection = specularDirection;
            }
            float brdfPDF = mat.PDF(incomingDirection, intersectionNormalLobe.ToLocal(brdfDirection));


            if (solidLightInScene) {
                // evaluate brdf color
                auto [ brdfColor, hittedLightFromBRDF, hittedLightFromBRDFPos] = EvaluateColor(Ray(surfacePoint, brdfDirection, EPSILON_RAY));
                brdfColor *= mat.EvaluateBRDF(hitLobeFrame.ToLocal(brdfDirection), incomingDirection) * std::max(0.0f, Dot(hitLobeFrame.mZ, brdfDirection)) / brdfPDF;

                // MIS
                float pdfAsFromBRDF = mat.PDF(incomingDirection, hitLobeFrame.ToLocal(lightDirection));
                float pdfAsFromLight = !hittedLightFromBRDF ? 0.0f : hittedLightFromBRDF->PDF(surfacePoint, hittedLightFromBRDFPos);
                if (std::isnan(pdfAsFromLight) || pdfAsFromLight < 0) pdfAsFromLight = 0;
                float misWeightBRDF = brdfPDF / (brdfPDF + pdfAsFromLight);
                float misWeightLight = lightPDF / (lightPDF + pdfAsFromBRDF);
                
                acum += throughput * brdfColor * misWeightBRDF;
                acum += throughput * colorFromLights * misWeightLight;
            }
            else {
                acum += throughput * colorFromLights;
            }


            /// russian roulete
            float survivalProb = std::min(1.0f, throughput.Max());
            throughput /= survivalProb;
            if (mRandomGenerator.GetFloat() >= survivalProb) {
                break;
            }

            /// next bounce
            Vec3f nextBounceDirection;
            float nextBouncePDF;
            /*/ <= switch between reuse ray and generate new onne
            nextBounceDirection = brdfDirection;
            nextBouncePDF = brdfPDF;
            /*/
            // diffuse direction
            Vec3f nextBounceRandomDirection = mRandomGenerator.CosineSampleHemisphere();
            nextBouncePDF = mRandomGenerator.CosineHemispherePdf(nextBounceRandomDirection);
            CoordinateFrame nextBounceLobeFrame;
            nextBounceLobeFrame.SetFromZ(intersection->normal);
            nextBounceDirection = nextBounceLobeFrame.ToWorld(nextBounceRandomDirection);
            /**/
            Ray nextBounceRay = Ray(surfacePoint, nextBounceDirection, EPSILON_RAY);
            ray = nextBounceRay;
            float cosTheta = Dot(hitLobeFrame.mZ, nextBounceDirection);
            throughput *= mat.EvaluateBRDF(hitLobeFrame.ToLocal(nextBounceDirection), incomingDirection) * cosTheta / nextBouncePDF;

            ray = Ray(surfacePoint, nextBounceDirection, EPSILON_RAY);
        }

        return acum;
    }

    std::tuple<Vec3f, const AbstractLight*, Vec3f> EvaluateColor(Ray ray) {
        auto intersection = mScene.FindClosestIntersection(ray);
        if (intersection) {
            const Vec3f surfacePoint = ray.origin + ray.direction * intersection->distance;

            if (intersection->lightID >= 0) {
                const AbstractLight* light = mScene.GetLightPtr(intersection->lightID);
                return { Vec3f(light->Evaluate(ray.direction)), light, surfacePoint };
            }
        }
        else {
            auto background = mScene.GetBackground();
            if (background) {
                return { background->Evaluate(ray.direction), background, Vec3f(0)};
            }
        }
        return { Vec3f(0), nullptr, Vec3f(0) };
    }
};
