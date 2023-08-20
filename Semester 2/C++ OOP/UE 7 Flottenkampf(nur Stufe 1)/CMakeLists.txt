cmake_minimum_required(VERSION 3.24)
project(untitled23)

set(CMAKE_CXX_STANDARD 17)

add_executable(untitled23 main.cpp Flotte.h Flotte.cpp Schiff.h Jaeger.h Zerstoerer.h Kreuzer.h Spiel.h Spiel.cpp Schiff.cpp Jaeger.cpp Zerstoerer.cpp Kreuzer.cpp)

target_link_libraries(untitled23 PRIVATE winmm)
