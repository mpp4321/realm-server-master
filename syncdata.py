import os
from shutil import copy2

def todat(x):
    return os.path.splitext(x)[0] + '.dat'
    
serverdata = os.path.abspath(\
    os.path.join(os.getcwd(), "Resources/GameData/"))

clientdata = os.path.abspath(\
    os.path.join(os.getcwd(), "../realm-client-master/src/kabam/rotmg/assets/"))
    
mappeddata = (list( \
        map( todat, os.listdir(serverdata) )
        )
     )

for (a, b) in zip(os.listdir(serverdata), mappeddata):
    copy2(os.path.join(serverdata, a), os.path.join(clientdata, b))
    print("Moved {} to {}", a, b)