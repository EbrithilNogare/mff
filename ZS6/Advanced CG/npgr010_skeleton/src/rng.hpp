#pragma once

#include <vector>
#include <cmath>

#if defined(_MSC_VER)
#   if (_MSC_VER < 1600)
#       define LEGACY_RNG
#   endif
#endif

#if !defined(LEGACY_RNG)

#include <random>
class Rng
{
public:
    Rng(int aSeed = 1234):
        mRng(aSeed)
    {}

    int GetInt()
    {
        return mDistInt(mRng);
    }

    uint GetUint()
    {
        return mDistUint(mRng);
    }

    float GetFloat()
    {
        return mDistFloat(mRng);
    }

    Vec2f GetVec2f()
    {
        float a = GetFloat();
        float b = GetFloat();

        return Vec2f(a, b);
    }

    Vec3f GetVec3f()
    {
        float a = GetFloat();
        float b = GetFloat();
        float c = GetFloat();

        return Vec3f(a, b, c);
    }

    Vec2f GetRandomOnTriangle() {
        auto out = GetVec2f();
        if (out.x + out.y > 1)
            out = Vec2f(1 - out.x, 1 - out.y);
        return out;
    }

    Vec3f GetRandomOnSphere() {
        auto u = GetFloat();
        auto v = GetFloat();
        auto theta = u * 2.0 * PI_F;
        auto phi = acos(2.0 * v - 1.0);
        auto r = cbrt(GetFloat());
        auto sinTheta = sin(theta);
        auto cosTheta = cos(theta);
        auto sinPhi = sin(phi);
        auto cosPhi = cos(phi);
        float x = r * sinPhi * cosTheta;
        float y = r * sinPhi * sinTheta;
        float z = r * cosPhi;
        return Normalize(Vec3f(x, y, z));
    }

    Vec3f GetRandomOnHemiSphere(Vec3f direction) {
        auto toReturn = GetRandomOnSphere();
        if (Dot(toReturn - direction, toReturn) < 0)
            toReturn = -toReturn;
        return toReturn;
    }

private:

    std::mt19937_64 mRng;
    std::uniform_int_distribution<int>    mDistInt;
    std::uniform_int_distribution<uint>   mDistUint;
    std::uniform_real_distribution<float> mDistFloat;
};

#else

template<unsigned int rounds>
class TeaImplTemplate
{
public:
    void Reset(
        uint aSeed0,
        uint aSeed1)
    {
        mState0 = aSeed0;
        mState1 = aSeed1;
    }

    uint GetImpl(void)
    {
        unsigned int sum=0;
        const unsigned int delta=0x9e3779b9U;

        for (unsigned int i=0; i<rounds; i++)
        {
            sum+=delta;
            mState0+=((mState1<<4)+0xa341316cU) ^ (mState1+sum) ^ ((mState1>>5)+0xc8013ea4U);
            mState1+=((mState0<<4)+0xad90777dU) ^ (mState0+sum) ^ ((mState0>>5)+0x7e95761eU);
        }

        return mState0;
    }

private:

    uint mState0, mState1;
};

typedef TeaImplTemplate<6>  TeaImpl;

template<typename RandomImpl>
class RandomBase
{
public:

    RandomBase(int aSeed = 1234)
    {
        mImpl.Reset(uint(aSeed), 5678);
    }

    uint  GetUint()
    {
        return getImpl();
    }

    float GetFloat()
    {
        return (float(GetUint()) + 1.f) * (1.0f / 4294967297.0f);
    }

    Vec2f GetVec2f   (void)
    {
        // cannot do return Vec2f(getF32(), getF32()) because the order is not ensured
        float a = GetFloat();
        float b = GetFloat();
        return Vec2f(a, b);
    }

    Vec3f GetVec3f   (void)
    {
        float a = GetFloat();
        float b = GetFloat();
        float c = GetFloat();
        return Vec3f(a, b, c);
    }

    //////////////////////////////////////////////////////////////////////////
    void StoreState(
        uint *oState1,
        uint *oState2)
    {
        mImpl.StoreState(oState1, oState2);
    }

    void LoadState(
        uint aState1,
        uint aState2,
        uint aDimension)
    {
        mImpl.LoadState(aState1, aState2, aDimension);
    }

protected:

    uint getImpl(void)
    {
        return mImpl.GetImpl();
    }

private:

    RandomImpl mImpl;
};

typedef RandomBase<TeaImpl> Rng;

#endif
