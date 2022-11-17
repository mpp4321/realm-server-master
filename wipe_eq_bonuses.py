import json
import os
import re
import xml.etree.ElementTree as ET

homePath = input()
matcher = re.compile("char\.\d+.\d+\.file")

assert (matcher.match("char.0.1.file") != None)


def l(f):
    return os.path.join(homePath, f)


# Grab files from Database/char.*.*.file
files = list(filter(lambda x: matcher.match(x) != None, os.listdir(homePath)))

for file in files:
    with open(l(file), 'r') as file_w:
        fileContents = file_w.read()
        try:
            xmlTree = ET.fromstring(fileContents)
        except:
            continue
        itemDatas = xmlTree.find("./ItemDatas")
        itemDataJson = json.loads("[" + itemDatas.text + "]")
        for element in itemDataJson:
            element["ExtraStatBonuses"] = {}

            if "ItemComponent" in element:
                del element["ItemComponent"]

        itemDatas.text = ','.join(list(
            map(lambda x: json.dumps(x), itemDataJson)))

        file_w.close()
        with open(l(file), 'w') as file_w:
            # print(b"\r\n".join(ET.tostringlist(xmlTree)))
            file_w.write(''.join(ET.tostringlist(xmlTree, 'unicode')))
