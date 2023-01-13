name=jkl
gcc32=gcc -m32
dllflg=$(INDIR) -s -Os -shared -o
exeflg=$(INDIR) -s -Os -mwindows -o
rc=windres
rc32=windres -F pe-i386
rco=./obj/$(name).rc.o
rco32=./obj/$(name)x86.rc.o
all: ./bin/ ./obj/ objx64 objx86 dllDEL $(rco) $(rco32)
	gcc -D ARCH $(rco) ./obj/$(name).o $(dllflg) ./bin/$(name).dll
	$(gcc32) $(rco32) ./obj/$(name)x86.o $(dllflg) ./bin/$(name)x86.dll
	gcc -D ARCH $(rco) ./obj/$(name).exe.o $(exeflg) ./bin/$(name).exe
	$(gcc32) $(rco32) ./obj/$(name)x86.exe.o -Wl,-Bstatic -lpthread $(exeflg) ./bin/$(name)x86.exe
	
x86o: ./obj/ dllDEL $(rco32)
	$(gcc32) $(name).cxx -c $(dllflg) ./obj/$(name).o
	$(gcc32) $(name).c -c $(exeflg) ./obj/$(name).exe.o
	$(gcc32) $(rco32) ./obj/$(name).o $(dllflg) $(name).dll
	$(gcc32) $(rco32) ./obj/$(name).exe.o $(exeflg) $(name).exe
	
x64o: ./obj/ objx64 dllDEL $(rco)
	gcc -D ARCH $(rco) ./obj/$(name).o $(dllflg) ./bin/$(name).dll
	gcc -D ARCH $(rco) ./obj/$(name).exe.o $(exeflg) ./bin/$(name).exe

objx86:
	$(gcc32) ./$(name).cxx -c $(dllflg) ./obj/$(name)x86.o
	$(gcc32) ./$(name)x86.c -c -Wl,-Bstatic -lpthread $(exeflg) ./obj/$(name)x86.exe.o

objx64:
	gcc -D ARCH ./$(name).cxx -c $(dllflg) ./obj/$(name).o
	gcc -D ARCH ./$(name).c -c $(exeflg) ./obj/$(name).exe.o

$(rco):
	$(rc) ./$(name).rc -o ./obj/$(name).rc.o

$(rco32):
	$(rc32) ./$(name).rc -o ./obj/$(name)x86.rc.o
	
./obj/:
	+@mkdir -p ./obj

./bin/:
	+@mkdir -p ./bin/

clean: dllDEL
	-@rm -f ./bin/$(name)*.exe || true
	-@rm -rf ./obj ./bin || true

dllDEL:
	-@rm -f ./bin/$(name)*.dll || true

re: clean all

rer: re run

zip: 
	upx -9 ./bin/*

run:
	./bin/$(name).exe