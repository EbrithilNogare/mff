#pragma once 

#include <utility>
#include <tuple>
#include <stdexcept>
#include "math.hpp"
#include "rng.hpp"

class Material
{
public:
    Material()
    {
        Reset();
    }

    void Reset()
    {
        mDiffuseReflectance = Vec3f(0);
        mPhongReflectance   = Vec3f(0);
        mPhongExponent      = 1.f;
    }

    /**
     * Randomly chooses an outgoing direction that is reflected from the material surface
     * Arguments:
     *  - incomingDirection = a normalized direction towards the previous (origin) point in the scene
     *  - rng = random generator
     * Returns:
     *  - a randomly sampled reflected outgoing direction
     *  - the intensity corresponding to the reflected light
     *  - the probability density (PDF) of choosing this direction
     */
    std::tuple<Vec3f, Vec3f, float> SampleReflectedDirection(const Vec3f& incomingDirection, Rng& rng) const {
        throw std::logic_error("Not implemented");
    }

    /**
     * Returns the probability density corresponding to sampleReflectedDirection,
     * i.e., what is the probability that calling sampleReflectedDirection would randomly choose the given outgoingDirection
     * Arguments:
     *  - incomingDirection = a normalized direction towards the previous (origin) point in the scene
     *  - outgoingDirection = the randomly sampled (normalized) outgoing direction
     */
    float PDF(const Vec3f& incomingDirection, const Vec3f& outgoingDirection) const {
        float diffuseProbability = mDiffuseReflectance.Max();
        float specularProbability = mPhongReflectance.Max();
        float normalization = 1.f / (diffuseProbability + specularProbability);
        diffuseProbability *= normalization;
        specularProbability *= normalization;

        float diffusePDF = Rng::CosineHemispherePdf(outgoingDirection);
        if (std::isnan(diffusePDF))diffusePDF = 0;

        CoordinateFrame lobeFrame;
        Vec3f reflectedDir = incomingDirection - 2 * (Dot(incomingDirection, Vec3f(0, 0, 1))) * Vec3f(0,0,1);
        lobeFrame.SetFromZ(reflectedDir);
        Vec3f fixedOutgoingDirection = lobeFrame.ToLocal(outgoingDirection);
        float specularPDF = Rng::rndHemiCosNPDF(fixedOutgoingDirection, mPhongExponent);
        if (std::isnan(specularPDF))specularPDF = 0;

        return diffuseProbability * diffusePDF + specularProbability * specularPDF;
    }

    /**
     * Returns the intensity corresponding to the reflected light according to this material's BRDF
     * Arguments:
     *  - incomingDirection = a normalized direction towards the previous (origin) point in the scene
     *  - outgoingDirection = a normalized outgoing reflected direction
     */
    Vec3f EvaluateBRDF(const Vec3f& incomingDirection, const Vec3f& outgoingDirection) const {
        if (incomingDirection.z <= 0 && outgoingDirection.z <= 0) {
			return Vec3f(0);
        }
        Vec3f up = Vec3f(0, 0, 1);

        Vec3f diffuseComponent = mDiffuseReflectance / PI_F;
        
        // mDiffuseReflectance * Dot(up, incomingDirection);
        Vec3f r = 2 * Dot(up, incomingDirection) * up - incomingDirection;
        Vec3f glossyComponent = mPhongReflectance * (mPhongExponent+2)/ (2*PI_F) * pow(Dot(r, outgoingDirection), mPhongExponent);
		
        return diffuseComponent + glossyComponent;
    }

    Vec3f mDiffuseReflectance;
    Vec3f mPhongReflectance;
    float mPhongExponent;
};
