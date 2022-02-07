import os
import xml.etree.ElementTree as ET

hashSet = set()
stringHashSet = set()
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
            stringId = obj.attrib['id']
            if hexToDec in hashSet:
                print("Duplicate found: " + hex(hexToDec))
            hashSet.add(hexToDec)
            stringHashSet.add(stringId)
            localSet.add(hexToDec)
    files.append((file, localSet))

def add_valid_id(_type):
    hashSet.add(_type)

def is_string_id(stri):
    if stri in stringHashSet:
        return False
    else:
        return True

# requires hex to be valid int of base 16
def is_valid_id(hex):
    if hex in hashSet:
        return False
    else:
        return True

if __name__ == '__main__':
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
    while not is_valid_id(largest):
        largest += 1
    print("Next valid id: {}".format(hex(largest)))