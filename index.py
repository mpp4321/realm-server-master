import os
import xml.etree.ElementTree as ET

hashSet = set()
files = []
for file in os.listdir("./Resources/GameData"):
    localSet = set()
    # Get the XML of the file
    with open("./Resources/GameData/" + file, "r") as f:
        xml = f.read()
        # Parse the XML as XML object
        et = ET.fromstring(xml)
        for obj in et.findall('Object'):
            hexToDec = int(obj.attrib['type'], 16)
            if hexToDec in hashSet:
                print("Duplicate found: " + hex(hexToDec))
            hashSet.add(hexToDec)
            localSet.add(hexToDec)
    files.append((file, localSet))

# Let the user choose one of the strings in files
print("Choose a file: ")
for i in range(len(files)):
    print(str(i) + ": " + files[i][0])
choice = input("Enter a number: ")
fileChoice = files[int(choice)]
localFileSet = fileChoice[1]

# Find the largest number in localFileSet
largest = 0
for i in localFileSet:
    if i > largest:
        largest = i

print("Largest valid id: {}".format(hex(largest)))
a = input("Enter a hex value: ")
while a != "":
    if int(a, 16) in hashSet:
        print("That's a duplicate")
    else:
        print("That's a valid type")
    a = input("Enter a hex value: ")