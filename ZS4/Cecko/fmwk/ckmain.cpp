#include "ckmain.hpp"

#include <iomanip>
#include <iostream>

namespace cecko {

	template< typename AR>
	inline bool read_args(int argc, char** argv, AR&& arg_reader, std::string& input_fname, int& app_argc, char**& app_argv)
	{
		std::size_t aix = 1, apos = 0;

		auto getter = [argv, argc, &aix, &apos]() -> std::string {
			if (argv[aix][apos])
			{
				auto val = std::string(argv[aix] + apos);
				++aix;
				return val;
			}
			if (aix + 1 < argc)
			{
				auto val = std::string(argv[aix + 1]);
				aix += 2;
				return val;
			}
			return std::string();
		};

		while (aix < argc && (apos > 0 || argv[aix][0] == '-'))
		{
			// skip '-' if necessary
			if (apos == 0 && argv[aix][0] == '-')
				++apos;
			// read the option char
			char opt = argv[aix][apos];
			// move to the next char
			++apos;
			if (!argv[aix][apos])
			{
				++aix;
				apos = 0;
			}
			// call the reader, optionally moving through an argument
			auto rv = arg_reader(opt, getter);
			if (!rv)
			{
				std::cout << "Unrecognized switch \"" << opt << "\"" << std::endl;
				return false;
			}
		}

		if (aix >= argc)
		{
			std::cout << "cecko <input file>" << std::endl;
			return false;
		}

		input_fname = argv[aix];
		app_argc = argc - aix;
		app_argv = argv + aix;

		return true;
	}

	bool main_state_parser::dump_tables() const
	{
		out() << "========== tables ==========" << std::endl;
		the_tables.dump_tables(out());

		if (!covname.empty())
		{
			std::ofstream covf(covname);
			cd_.for_each([&covf](auto&& name, auto&& cc) {
				covf << std::setw(5) << cc.get() << "\t" << name << std::endl;
				});
		}
		if (!covlinename.empty())
		{
			std::ofstream covf(covlinename);
			std::ifstream inf(input_fname);
			loc_t infline = 0;
			cd_.for_each_line([&covf, &inf, &infline](auto&& line, auto&& lcd) {
				for (;;)
				{
					std::string ins;
					std::getline(inf, ins);
					++infline;
					//covf << std::setw(5) << infline << "\t";
					covf << ins;
					if (infline >= line)
					{
						std::string delim = "\t//# ";
						lcd.for_each([&covf, &delim](auto&& name) {
							covf << delim << name;
							delim = " ";
							});
						covf << std::endl;
						break;
					}
					covf << std::endl;
				}
				});
			for (;;)
			{
				std::string ins;
				std::getline(inf, ins);
				if (inf.fail())
					break;
				++infline;
				//covf << std::setw(5) << infline << "\t";
				covf << ins;
				covf << std::endl;
			}
		}
		return true;
	}

	bool main_state_parser::dump_coverage() const
	{
		out() << "========== coverage ==========" << std::endl;
		cd_.for_each([this](auto&& name, auto&& cc) {
			out() << "#" << std::setw(5) << cc.get() << " " << name << std::endl;
			});
		return true;
	}

	bool main_state_code::setup(int argc, char** argv)
	{
		auto arg_reader = [this](char opt, auto&& get_val) -> bool {
			switch (opt)
			{
			case 'a':
				if (!aname.empty())
					return false;
				aname = get_val();
				a_to_out = false;
				return true;
			case 'c':
				if (!covname.empty())
					return false;
				covname = get_val();
				return true;
			case 'd':
				if (!covlinename.empty())
					return false;
				covlinename = get_val();
				return true;
			case 'n':
				a_to_out = false;
				return true;
			case 'o':
				if (!oname.empty())
					return false;
				oname = get_val();
				return true;
			case 'z':
				if (!!outp_owner_)
					return false;
				{
					auto outname = get_val();
					outp_owner_ = std::make_unique<std::ofstream>(outname);
					if (!outp_owner_->good())
					{
						outp_owner_.reset();
						return false;
					}
					outp_ = &*outp_owner_;
					a_to_out = false;
				}
				return true;
			default:
				return false;
			}
		};

		return read_args(argc, argv, arg_reader, input_fname, app_argc, app_argv);
	}

	bool main_state_code::dump_code() const
	{
		if (a_to_out)
		{
			out() << "========== IR module ==========" << std::endl;
			std::stringstream oss;
			the_tables.dump_ir_module(oss);
			for (;;)
			{
				std::string lbuf;
				auto rc = !!std::getline(oss, lbuf);
				if (!rc)
					break;
				out() << "::: " << lbuf << std::endl;
			}
		}

		if (!aname.empty())
		{
			std::ofstream af(aname);
			if (!af.good())
			{
				std::cout << "Cannot open output file \"" << aname << "\"" << std::endl;
			}
			else
			{
				the_tables.dump_ir_module(af);
			}
		}

		if (!oname.empty())
		{
			auto oec = the_tables.write_bitcode_module(oname);
			if (!!oec)
			{
				std::cout << "Cannot open output file \"" << oname << "\": " << oec.message() << std::endl;
			}
			else
			{
				std::cout << "Module bitcode written into the file \"" << oname << "\"" << std::endl;
			}
		}
		return true;
	}

	bool main_state_code::run_code()
	{
		auto mainf = the_tables.globtable()->find_function("main");
		if (!mainf)
		{
			out() << "Cannot find main function." << std::endl;
			return false;
		}
		else
		{
			auto fnc = mainf->get_function_ir();
			irenv.run_main(fnc, app_argc, app_argv, out());
			return true;
		}
	}
}
