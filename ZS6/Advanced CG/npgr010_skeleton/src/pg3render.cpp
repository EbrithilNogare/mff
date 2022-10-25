#include <vector>
#include <cmath>
#include <cstdlib>
#include <chrono>
#include <algorithm>
#include "math.hpp"
#include "ray.hpp"
#include "geometry.hpp"
#include "camera.hpp"
#include "framebuffer.hpp"
#include "scene.hpp"
#include "pathtracer.hpp"
#include "config.hpp"

#include <omp.h>
#include <string>
#include <set>
#include <sstream>

//////////////////////////////////////////////////////////////////////////
// The main rendering function, renders what is in aConfig

float render(
    const Config &aConfig,
    int *oUsedIterations = NULL)
{
    // Set number of used threads
    omp_set_num_threads(aConfig.mNumThreads);

    // Create 1 renderer per thread
    typedef AbstractRenderer* AbstractRendererPtr;
    AbstractRendererPtr *renderers;
    renderers = new AbstractRendererPtr[aConfig.mNumThreads];

    for (int i=0; i<aConfig.mNumThreads; i++)
    {
        renderers[i] = CreateRenderer(aConfig, aConfig.mBaseSeed + i);

        renderers[i]->mMaxPathLength = aConfig.mMaxPathLength;
        renderers[i]->mMinPathLength = aConfig.mMinPathLength;
    }

    auto startT = std::chrono::high_resolution_clock::now();
    int iter = 0;

    // Rendering loop
    // Iterations based loop
    int globalCounter = 0;
#pragma omp parallel for
    for (iter=0; iter < aConfig.mIterations; iter++)
    {
        int threadId = omp_get_thread_num();
        renderers[threadId]->RunIteration(iter);

        // Print progress bar
#pragma omp critical
        {
            globalCounter++;
            const double progress   = (double)globalCounter / aConfig.mIterations;
            const int barCount      = 20;

            printf(
                "\rProgress:  %6.2f%% [", 
                100.0 * progress);
            for (int bar = 1; bar <= barCount; bar++)
            {
                const double barProgress = (double)bar / barCount;
                if (barProgress <= progress)
                    printf("|");
                else
                    printf(".");
            }
            printf("]");
            fflush(stdout);
        }
    }

    auto endT = std::chrono::high_resolution_clock::now();

    if (oUsedIterations)
        *oUsedIterations = iter+1;

    // Accumulate from all renderers into a common framebuffer
    int usedRenderers = 0;

    // With very low number of iterations and high number of threads
    // not all created renderers had to have been used.
    // Those must not participate in accumulation.
    for (int i=0; i<aConfig.mNumThreads; i++)
    {
        if (!renderers[i]->WasUsed())
            continue;

        if (usedRenderers == 0)
        {
            renderers[i]->GetFramebuffer(*aConfig.mFramebuffer);
        }
        else
        {
            Framebuffer tmp;
            renderers[i]->GetFramebuffer(tmp);
            aConfig.mFramebuffer->Add(tmp);
        }

        usedRenderers++;
    }

    // Scale framebuffer by the number of used renderers
    aConfig.mFramebuffer->Scale(1.f / usedRenderers);

    // Clean up renderers
    for (int i=0; i<aConfig.mNumThreads; i++)
        delete renderers[i];

    delete [] renderers;

    return float(std::chrono::duration_cast<std::chrono::milliseconds>(endT - startT).count()) / 1000.f; // in seconds
}

//////////////////////////////////////////////////////////////////////////
// Main

int main(int argc, const char *argv[])
{
    // Warns when not using C++11 Mersenne Twister
    PrintRngWarning();

    // Setups config based on command line
    Config config;
    ParseCommandline(argc, argv, config);

    // If number of threads is invalid, set 1 thread per processor
    if (config.mNumThreads <= 0)
        config.mNumThreads  = std::max(1, omp_get_num_procs());

    // When some error has been encountered, exit
    if (config.mScene == NULL)
        return 1;

    // Sets up framebuffer and number of threads
    Framebuffer fbuffer;
    config.mFramebuffer = &fbuffer;

    // Prints what we are doing
    printf("Scene:     %s\n", config.mScene->mSceneName.c_str());
    printf("Target:    %d iteration(s)\n", config.mIterations);

    // Renders the image
    printf("Running ...");
    fflush(stdout);
    float time = render(config);
    printf(" done in %.2f s\n", time);

    // Saves the image
    printf("Saving to: %s ... ", config.mOutputName.c_str());
    std::string extension = config.mOutputName.substr(config.mOutputName.length() - 3, 3);
    if (extension == "bmp")
    {
        fbuffer.SaveBMP(config.mOutputName.c_str(), 2.2f /*gamma*/);
        printf("done\n");
    }
    else if (extension == "hdr")
    {
        fbuffer.SaveHDR(config.mOutputName.c_str());
        printf("done\n");
    }
    else if (extension == "pfm")
    {
        fbuffer.SavePFM(config.mOutputName.c_str());
        printf("done\n");
    }
    else
        printf("Used unknown extension %s\n", extension.c_str());

    // Scene cleanup
    delete config.mScene;

    // debug
    // getchar(); // Wait for pressing the enter key on the command line

    return 0;
}
