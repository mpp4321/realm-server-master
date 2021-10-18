import xml.etree.ElementTree as ET
import glob, os

base_format_file = "./base_format.xml"

with open(base_format_file, 'r') as base_file:
    index = int(input("enter first index: "), 16)
    projectileNameFormat = input("enter base name: ")
    fileName = input("enter xml file name: ")
    fileIndex = input("enter the y coordinate hex: ")
    numToGen = int(input("enter the number to gen up to 16: "))
    c = base_file.read()
    with open("output.txt", 'a') as outputf:
        for i in range(numToGen):
            contents = c
            contents = contents.replace("{ID}", projectileNameFormat + str(i))
            contents = contents.replace("{TYPE}", hex(index + i))
            contents = contents.replace("{FILENAME}", fileName)
            contents = contents.replace("{INDEX}", fileIndex + hex(i)[2:])
            outputf.write(contents + "\n")
    
    