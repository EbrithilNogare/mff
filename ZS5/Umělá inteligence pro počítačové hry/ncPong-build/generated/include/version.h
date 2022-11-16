#ifndef NCPROJECT_VERSION
#define NCPROJECT_VERSION

struct VersionStrings
{
	static char const * const Version;
	static char const * const GitRevCount;
	static char const * const GitShortHash;
	static char const * const GitLastCommitDate;
	static char const * const GitBranch;
	static char const * const GitTag;
	static char const * const CompilationDate;
	static char const * const CompilationTime;
};

#endif