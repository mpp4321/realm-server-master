import os
import pandas as pd
import xml.etree.ElementTree as ET

equipmentData = []

def find_or_default(et, tag, default):
    value = et.find(tag)
    if value is None:
        return default
    return value

def find_text(obj, tag):
    value = obj.find(tag)
    if value is None:
        return ""
    return value.text

def int_or_one(txt):
    try:
        return int(txt)
    except:
        return 1

def float_or_zero(txt):
    try:
        return float(txt)
    except:
        return 0

def construct_projectiles(obj):
    projectiles = obj.findall('Projectile')
    if projectiles is None or len(projectiles) == 0:
        return (0, 0)
    num_shots = int_or_one(find_text(obj, 'NumProjectiles'))
    rof = float_or_zero(find_text(obj, 'RateOfFire'))
    toReturn = []
    for projectile in projectiles:
        if projectile.find("MinDamage") is None:
            toReturn.append(int(projectile.find("Damage").text))
            continue
        projectile_damage = (int(projectile.find("MinDamage").text) + int(projectile.find("MaxDamage").text)) / 2
        toReturn.append(projectile_damage)
    totalDamageOfShot = 0
    for i in range(num_shots):
        damageThisShot = toReturn[i % len(toReturn)]
        totalDamageOfShot += damageThisShot
    return (totalDamageOfShot, rof*totalDamageOfShot)

def find_stats(obj):
    stats = obj.findall('ActivateOnEquip')
    toReturn = list(map(lambda x: 0, range(9)))
    if len(stats) == 0:
        return toReturn
    for stat in stats:
        statAttrib = stat.attrib['stat']
        amount = stat.attrib['amount']
        try:
            toReturn[int(statAttrib)] = int(amount)
        except:
            print(statAttrib, obj.attrib['id'])
    return toReturn

for file in os.listdir("./Resources/GameData"):
    with open("./Resources/GameData/" + file, "r") as f:
        xml = f.read()
        # Parse the XML as XML object
        et = ET.fromstring(xml)
        for obj in et.findall("Object"):
            classObj = obj.find("Class")
            if classObj is None: continue
            classStr = classObj.text
            slottype = obj.find("SlotType")
            bagtype = obj.find("BagType")
            
            if bagtype is None:
                bagtype = "0"
            else:
                bagtype = bagtype.text

            if slottype is None: 
                slottype = "None"
            else: slottype = slottype.text

            if classStr == "Equipment":
                # Append data to equipmentData
                equipmentData.append(
                    (
                        obj.attrib["type"],
                        obj.attrib["id"],
                        slottype,
                        bagtype,
                        *construct_projectiles(obj),
                        *find_stats(obj)
                    )
                )

df = pd.DataFrame(equipmentData, columns=["Type", "Id", "Slot Type", "Bag Type", "Avg P Damage", "Rof Damage", "Health", "Mana", "Attack", "Defense", "Speed", "Dexterity", "Wisdom", "Vitality", "Protection"])
df.to_excel("./EquipmentStats.xlsx")