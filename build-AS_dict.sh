cd Dictionaries-origin
if [[ "$1" == "" ]]; then
	echo Usage: /build-AS_dict.sh {type} {type} {type} 
	echo -e "Example: \n\t/build-AS_dict.sh big custom\nwill create dictionary with big and custom dictionaries."
	echo If pass {type} as all it will combine all dictionaries to AS_dict.txt.
	echo -e "Possible {type}s(left to filename):\n"
else
	echo Creating empty AS_dict.txt
	> ../AS_dict.txt # temp
	echo Converting AS_dict.txt to UTF-8.
	iconv -c -f utf-8 -t utf-8 ../AS_dict.txt
fi
all=0
changed=0
if [[ "${1,,}" == "all" ]] || [[ "${2,,}" == "all" ]] || [[ "${3,,}" == "all" ]]; then
	all=1
fi
for f in *.txt; do
	if [[ "$f" == *"big"*  ]]; then
		if [[ "${1,,}" == "big" ]] || [[ "${2,,}" == "big" ]] || [[ "${3,,}" == "big" ]] || [[ $all == 1 ]];  then
			echo "-->Adding $f to AS_dict.txt..."
			changed=1
			cat $f >> ../AS_dict.txt
			printf "\r\n" >> ../AS_dict.txt
		elif [[ "$1" == "" ]]; then
			echo BIG: $f
		fi
	elif [[ "$f" == *!* ]]; then
		if [[ "${1,,}" == "tiny" ]] || [[ "${2,,}" == "tiny" ]] || [[ "${3,,}" == "tiny" ]] || [[ $all == 1 ]]; then
			echo "-->Adding $f to AS_dict.txt..."
			changed=1
			cat $f >> ../AS_dict.txt
			printf "\r\n" >> ../AS_dict.txt
		elif [[ "$1" == "" ]]; then
			echo tiny: $f
		fi
	else
		if [[ "${1,,}" == "custom" ]] || [[ "${2,,}" == "custom" ]] || [[ "${3,,}" == "custom" ]] || [[ $all == 1 ]]; then
			echo "-->Adding $f to AS_dict.txt..."
			changed=1
			cat $f >> ../AS_dict.txt
			printf "\r\n" >> ../AS_dict.txt
		elif [[ "$1" == "" ]]; then
			echo Custom: $f
		fi
	fi
done
if [[ $changed == 1 ]]; then
	echo Done.
fi