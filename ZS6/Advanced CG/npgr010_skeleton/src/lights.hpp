#pragma once

#include <vector>
#include <cmath>
#include <utility>
#include <tuple>
#include <stdexcept>
#include "math.hpp"
#include "rng.hpp"


class AbstractLight
{
public:

    /**
     * Randomly chooses a point on the light source
     * Arguments:
     *  - origin = our current position in the scene
     *  - rng = random generator
     * Returns:
     *  - a randomly sampled point on the light source
     *  - the illumination intensity corresponding to the sampled direction
     *  - the probability density (PDF) of choosing this point
     */
    virtual std::tuple<Vec3f, Vec3f, float> SamplePointOnLight(const Vec3f& origin, Rng& rng) const {
        throw std::logic_error("Not implemented");
    }

    /**
     * Returns the probability density corresponding to samplePointOnLight,
     * i.e., what is the probability that calling samplePointOnLight would randomly choose the given lightPoint
     * Arguments:
     *  - origin = our current position in the scene
     *  - lightPoint = the randomly sampled point on the light source
     */
    virtual float PDF(const Vec3f& origin, const Vec3f& lightPoint) const {
        throw std::logic_error("Not implemented");
    }

    /**
     * Returns the illumination intensity in the given direction
     * Arguments:
     *  - direction = direction towards the light source
     */
    virtual Vec3f Evaluate(const Vec3f& direction) const {
        throw std::logic_error("Not implemented");
    }

    virtual ~AbstractLight() = default;

    virtual bool hasVolume() const { throw std::logic_error("Not implemented"); }
};

//////////////////////////////////////////////////////////////////////////
class AreaLight : public AbstractLight
{
public:

    AreaLight(
        const Vec3f &aP0,
        const Vec3f &aP1,
        const Vec3f &aP2)
    {
        p0 = aP0;
        e1 = aP1 - aP0;
        e2 = aP2 - aP0;

        Vec3f normal = Cross(e1, e2);
        float len    = normal.Length();
        mInvArea     = 2.f / len;
        mFrame.SetFromZ(normal);
    }

    std::tuple<Vec3f, Vec3f, float> SamplePointOnLight(const Vec3f& origin, Rng& rng) const override {
        Vec3f normal = Cross(e1, e2);
        auto rndTriangle = rng.GetRandomOnTriangle();
        Vec3f randomPoint3D = e1 * rndTriangle.x + e2 * rndTriangle.y + p0;
        Vec3f outgoingDirection = origin - randomPoint3D;
        float distanceSquared = outgoingDirection.LenSqr();
        float lambertCosineLaw = Dot(normal, outgoingDirection) / (normal.Length() * outgoingDirection.Length());
        
        return { randomPoint3D, mRadiance / distanceSquared * lambertCosineLaw,  PDF(origin, randomPoint3D)};
    }

    float PDF(const Vec3f& origin, const Vec3f& lightPoint) const {
        return mInvArea;
    }


    Vec3f Evaluate(const Vec3f& direction) const {
        return mRadiance;
    }

    bool hasVolume() const override { return true; }

public:
    Vec3f p0, e1, e2;
    CoordinateFrame mFrame;
    Vec3f mRadiance;
    float mInvArea;
};

//////////////////////////////////////////////////////////////////////////
class PointLight : public AbstractLight
{
public:

    PointLight(const Vec3f& aPosition)
    {
        mPosition = aPosition;
    }

    std::tuple<Vec3f, Vec3f, float> SamplePointOnLight(const Vec3f& origin, Rng& rng) const override {
        Vec3f outgoingDirection = origin - mPosition;
        float distanceSquared = outgoingDirection.LenSqr();

		return {mPosition, mIntensity / distanceSquared, PDF(origin, mPosition)};
    }
    
    float PDF(const Vec3f& origin, const Vec3f& lightPoint) const {
        return 1.0f;
    }

    Vec3f Evaluate(const Vec3f& direction) const {
        return mIntensity;
    }

    bool hasVolume() const override { return false; }

public:

    Vec3f mPosition;
    Vec3f mIntensity;
};

//////////////////////////////////////////////////////////////////////////
class BackgroundLight : public AbstractLight
{
public:
    BackgroundLight()
    {
        mBackgroundColor = Vec3f(135, 206, 250) / Vec3f(255.f);
        mRadius = 100.f; // a radius big enough to cover the whole scene
    }

    std::tuple<Vec3f, Vec3f, float> SamplePointOnLight(const Vec3f& origin, Rng& rng) const override {
        auto rndSphere = rng.UniformSampleSphere() * mRadius;
        Vec3f outgoingDirection = origin - rndSphere;
        float distanceSquared = outgoingDirection.LenSqr();
        return { rndSphere, mBackgroundColor / distanceSquared, PDF(origin, rndSphere)};
    }

    float PDF(const Vec3f& origin, const Vec3f& lightPoint) const {
        return 1 / (4 * PI_F * pow(mRadius, 2));
    }

    Vec3f Evaluate(const Vec3f& direction) const {
        return mBackgroundColor;
    }
    
    bool hasVolume() const override { return true; }

public:

    Vec3f mBackgroundColor;
    float mRadius; // we model the background light as a huge sphere around the whole scene, with a given radius
};
