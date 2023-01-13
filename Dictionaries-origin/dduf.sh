#!/bin/bash
woaft="words after ->"
wobtw="words between ====> & <===="
if [[ "$1" == "" ]]; then
	echo Duplicates finder in Mahou dictionaries.
	echo Usage:
	echo dduf.sh [dictionary1] [dictionary2] [scantype] [threads] [write-exclusive] [no-out]
	echo scantype by default is:
	echo "0(or any) - [from dictionary1's {$wobtw} in dictionary2's $woaft]"
	echo other scantypes:
	echo "1 - [from dictionary1's {$woaft} in dictionary2's {$wobtw}]"
	echo "2 - [from dictionary1's {$woaft} in dictionary2's {$woaft}]"
	echo "3 - [from dictionary1's {$wobtw} in dictionary2's {$wobtw}]"
	echo threads is number of lines processed by script at time, default 4.
	echo write-exclusive if not 1, script will write exclusive matches from dictionary1 by scantype.
	echo no-out if not null, script won\'t print \"Scanning...\" messages.
else 
	scan() {
		if [[ "$1" != "" ]]; then 
			if [[ "$necho" == "" ]]; then
				echo Scanning: "$1 on thread $2"
			fi
			fix=`echo "$1" | cut -d' ' -f1 | sed -r 's/-/\\\\-/g'` # fix for -, it by any way(even in quotes) determined as grep's switch...
			vas=$(grep "^$fix\s" "$tmp2")
			if [[ $? -eq 0 ]]; then
				echo -e "Duplicate: [$fix]:\n\tfrom [$dict1]{$info1}\n\tin [$dict2]{$info2}:\n$vas" >> .duplicate
			elif [[ "$nexcl" == 1 ]]; then
				echo -e "Exclusive: [$fix]:\n\tfrom [$dict1]{$info1}\n\tin [$dict2]{$info2}:\n$vas" >> .exclusive
			fi
		fi
	}
	nexcl="$5"
	necho="$6"
	> .duplicate
	if [[ "$5" == 1 ]]; then > .exclusive ; fi
	STARTTIME=$(date +%s)
	dict1="$1"
	dict2="$2"
	threads=4
	if [[ "$4" != "" ]]; then
		threads="$4"
	fi
	awk '/->/ { print $0" Line:" NR} ' "$dict1" | sed -re 's/->(.+)/\1/g' > .tmp1short
	awk '/====>/ { print $0" Line:" NR}' "$dict1" | sed -re 's/====>(.+)<====/\1/g' > .tmp1big
	awk '/->/ { print $0" Line:" NR}' "$dict2" | sed -re 's/->(.+)/\1/g' > .tmp2short
	awk '/====>/ { print $0" Line:" NR}' "$dict2" | sed -re 's/====>(.+)<====/\1/g' > .tmp2big
	tmp1=".tmp1big"
	tmp2=".tmp2short"
	info1="$wobtw"
	info2="$woaft"
	mode=0
	if [[ "$3" != "" ]]; then
		mode="$3"
	fi
	if [[ "$mode" == 1 ]]; then
		info1="$woaft"
		info2="$wobtw"
		tmp1=".tmp1short"
		tmp2=".tmp2big"
	elif [[ "$mode" == 2 ]]; then
		info1="$woaft"
		info2="$woaft"
		tmp1=".tmp1short"
		tmp2=".tmp2short"
	elif [[ "$mode" == 3 ]]; then
		info1="$wobtw"
		info2="$wobtw"
		tmp1=".tmp1big"
		tmp2=".tmp2big"
	fi
	echo -e "MODE: $mode\nfrom $dict1-[$info1]\nin $dict2-[$info2]"
	exec 5<"$tmp1"
	while read l1 <&5; do
		if [[ $threads != 1 ]]; then
			scan "$l1" 1 &
			for p in $(seq 2 $threads); do
				l="l$p"
				read "$l" <&5
				scan "${!l}" "$p" &
			done
		else 
			scan "$l1" 1
		fi
		wait
	done
	exec 5<&-
	rm .tmp1* .tmp2*
	ENDTIME=$(date +%s)
	echo "Done in $(($ENDTIME - $STARTTIME)) seconds..."
fi