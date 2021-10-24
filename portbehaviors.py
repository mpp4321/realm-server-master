import os
from shutil import copy2
import fileinput
import re

os.chdir("PortingDatas/")
    
rs = [
    (r'\.Init', 'db.Init'),
    (r'State\([^\"]', 'State("base",'),
    (r'TimedTransition\((\d+), (.+)\)', r'TimedTransition(\2, \1)'),
    (r'projectileIndex', 'index'),
    (r'coolDown', 'cooldown'),
    (r'(\d*\.\d+)(?=[^f\d])', r'\1f'),
    (r'ItemType', 'LootType'),
    (r'HpLessTransition', 'HealthTransition'),
    (r'EntitiesNotExistsTransition\((.+), (\d+), (.+)\)', r'EntitiesNotExistsTransition(\2, \3, \1)'),
    (r'EntityNotExistsTransition\((.+), (\d+), (.+)\)', r'EntitiesNotExistsTransition(\2, \3, \1)'),
    (r'EntitiesWithinTransition\((.+), (.+),', r'EntitiesWithinTransition(\2, \1,'),
    (r'EntityExistsTransition\((.+), (.+),', r'EntitiesWithinTransition(\2, \1,')
    
]
    
for file in os.listdir(os.getcwd()):
    className = file.split(".")[1]
    for line in fileinput.input(os.path.join(os.getcwd(), file), inplace=True):
        for r in rs:
            if 'namespace' in line:
                line = line.replace("wServer.logic", "RotMG.Game.Logic.Database")
            if 'partial' in line:
                line = line.replace("partial ", "")
                line = line.replace("BehaviorDb", className)
            line = re.sub(*r, line)
        print(line.replace('\n', ''))