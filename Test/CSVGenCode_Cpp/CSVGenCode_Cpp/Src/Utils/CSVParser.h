#ifndef     _CSVPARSER_HPP_
# define    _CSVPARSER_HPP_

# include <stdexcept>
# include <string>
# include <vector>
# include <list>
# include <sstream>

namespace csv
{
	class Error : public std::runtime_error
	{

	public:
		Error(const std::string &msg) :
			std::runtime_error(std::string("CSVparser : ").append(msg))
		{
		}
	};

	class Row
	{
	public:
		Row(const std::vector<std::string> &);
		~Row(void);

	public:
		unsigned int Size(void) const;
		void Push(const std::string &);
		bool Set(const std::string &, const std::string &);

	private:
		const std::vector<std::string> _header;
		std::vector<std::string> _values;

	public:
		double ReadDouble(unsigned int idx) const {
			return atof(this->operator[](idx).c_str());
		}
		float ReadFloat(unsigned int idx) const {
			return (float)atof(this->operator[](idx).c_str());
		}
		int ReadInt32(unsigned int idx) const {
			return atoi(this->operator[](idx).c_str());
		}
		long long ReadInt64(unsigned int idx) const {
			return atoll(this->operator[](idx).c_str());
		}
		unsigned int ReadUInt32(unsigned int idx) const {
			return (unsigned int)atoi(this->operator[](idx).c_str());
		}
		unsigned long long ReadUInt64(unsigned int idx) const {
			return (unsigned long long)atoll(this->operator[](idx).c_str());
		}
		const std::string ReadString(unsigned int idx) const {
			return this->operator[](idx);
		}

		template<typename T>
		const T GetValue(unsigned int pos) const
		{
			if (pos < _values.size())
			{
				T res;
				std::stringstream ss;
				ss << _values[pos];
				ss >> res;
				return res;
			}
			throw Error("can't return this value (doesn't exist)");
		}
		const std::string operator[](unsigned int) const;
		const std::string operator[](const std::string &valueName) const;
		friend std::ostream& operator<<(std::ostream& os, const Row &row);
		friend std::ofstream& operator<<(std::ofstream& os, const Row &row);
	};

	enum DataType {
		eFILE = 0,
		ePURE = 1
	};

	class CSVParser
	{

	public:
		CSVParser(const std::string &, const DataType &type = eFILE, char sep = ',');
		~CSVParser(void);

	public:
		double ReadDouble(unsigned int row, unsigned int col) const {
			return GetRow(row).ReadDouble(col);
		}
		float ReadFloat(unsigned int row, unsigned int col) const {
			return GetRow(row).ReadFloat(col);
		}
		int ReadInt32(unsigned int row, unsigned int col) const {
			return GetRow(row).ReadInt32(col);
		}
		long long ReadInt64(unsigned int row, unsigned int col) const {
			return GetRow(row).ReadInt64(col);
		}
		unsigned int ReadUInt32(unsigned int row, unsigned int col) const {
			return GetRow(row).ReadUInt32(col);
		}
		unsigned long long ReadUInt64(unsigned int row, unsigned int col) const {
			return GetRow(row).ReadUInt64(col);
		}
		const std::string ReadString(unsigned int row, unsigned int col) const {
			return GetRow(row).ReadString(col);
		}

		const std::string& GetField(unsigned int row, unsigned int col)const{
			return GetRow(row)[col];
		}
		Row &GetRow(unsigned int row) const;
		unsigned int RowCount(void) const;
		unsigned int ColCount(void) const;
		std::vector<std::string> GetTitle(void) const;
		const std::string GetTitle(unsigned int pos) const;
		const std::string &GetFileName(void) const;

	public:
		bool deleteRow(unsigned int row);
		bool addRow(unsigned int pos, const std::vector<std::string> &);
		void sync(void) const;

	protected:

		void parseHeader(void);
		void parseContent(void);

	private:
		std::string _file;
		const DataType _type;
		const char _sep;
		std::vector<std::string> _originalFile;
		std::vector<std::string> _header;
		std::vector<Row *> _content;

	public:
		Row &operator[](unsigned int row) const;
	};
}

#endif /*!_CSVPARSER_HPP_*/
