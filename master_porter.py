from types import NoneType
from PIL import Image
import xml.etree.ElementTree as ET
import os
from index import is_string_id, is_valid_id

hexPosition = 0x0
assetFileDirectory = "../realm-client-master/src/kabam/rotmg/assets/"

def chooseFile(directory):
    dict_files = {}
    char_cur = ord('a')
    for file in os.listdir(assetFileDirectory):
        # check to see if file type is png
        if file.endswith(".png"):
            dict_files[chr(char_cur)] = file
            char_cur += 1
            if char_cur > ord('z'):
                char_cur = ord('A')
    print("Choose a file to convert:")
    for key, value in dict_files.items():
        print(key, value)
    file_choice = input("Enter a letter: ")
    if file_choice in dict_files:
        return dict_files[file_choice]
    else:
        print("Invalid file choice")
        return None

def figure_size_from_name(name):
    if '8x8' in name:
        return (8, 8)
    if '40x40' in name:
        return (40, 40)
    if 'Big' in name:
        return (16, 16)
    if '32x32' in name:
        return (32, 32)
    if '64x64' in name:
        return (64, 64)
    if 'Small' in name:
        return (8, 8)
    if '9x9' in name:
        return (9, 9)
    if '4x4' in name:
        return (4, 4)
    return (8, 8)

def rect_from_hex(hex, w, h):
    y_cord = hex // 16
    x_cord = hex % 16
    xy = (x_cord * w, y_cord * h)
    return (*xy, xy[0] + w, xy[1] + h)

def get_image_section(path, hex_sec):
    figured_size = figure_size_from_name(path)
    image = Image.open(os.path.join(assetFileDirectory, path))

    if image.size[0] % figured_size[0] != 0 or image.size[1] % figured_size[1] != 0:
        print("Image size is not a multiple of the figure size")
        return None

    rect_to_do = rect_from_hex(hex_sec, *figured_size)
    output = image.crop(rect_to_do)
    output = output.resize(figured_size)
    return output

#file = chooseFile(assetFileDirectory)
#get_image_section(file, hexPosition).show()

def port_some_images(dir_of_pngs, xml_of_stuff):
    def get_img_name(txt):
        if "Embed" in txt:
            return f"EmbeddedAssets_{txt}_.png" 
        return f"EmbeddedAssets_{txt}Embed_.png"

    validSizes = set( [(8, 8)] )
    imagesBySize = {}
    for validSize in list(validSizes):
        imagesBySize[validSize] = [Image.new(mode="RGBA", size=(0x10 * validSize[0], 0xFF * validSize[1])), 0]

    with open(xml_of_stuff, "r") as f:
        xml = f.read()
        # Parse the XML as XML object
        et = ET.fromstring(xml)
        for obj in et.findall("Object"):
            obj_txt = obj.find('Texture')
            if obj_txt == None:
                continue
            obj_txtindx = obj_txt.find('Index')
            if obj_txtindx == None:
                continue
            obj_txtfile = obj_txt.find("File")
            if obj_txtfile == None:
                continue

            size = figure_size_from_name(obj_txtfile)
            if size in validSizes:
                imageArr = imagesBySize[size]
                file_name = get_img_name(obj_txtfile.text)
                img_path = os.path.join(dir_of_pngs, file_name)
                if not os.path.exists(img_path):
                    print("Missing image:", img_path)
                    continue
                print(img_path)
                crop_rect = rect_from_hex(int(obj_txtindx.text, 16), *size)
                image_reading = Image.open(img_path).copy()\
                    .crop(crop_rect)
                image_reading = image_reading.resize(size)
                paste_pos = rect_from_hex(imageArr[1], *size)
                print(obj_txtindx.text, crop_rect)
                imageArr[0].paste(image_reading, (paste_pos[0], paste_pos[1]))
                obj_txtfile.text = "TODO_TEXTURE_FILE"
                obj_txtindx.text = hex(imageArr[1])
                open("./missingres/output_after_images.xml", 'a').write(ET.tostring(obj, encoding='unicode'))
                imageArr[1] += 1
            pass
    
    i = 0
    for image in imagesBySize.values():
        image[0].save(f"image_rip/{i}.png")
        i += 1
    pass

def port_some_xml(dir_of_xmls, filters=[""]):
    for file in os.listdir(dir_of_xmls):
        if not file.endswith(".xml"):
            continue
        with open(os.path.join(dir_of_xmls, file), "r") as f:
            xml = f.read()
            # Parse the XML as XML object
            et = ET.fromstring(xml)
            for obj in et.findall("Object"):
                _id = obj.attrib["id"]
                _type = int(obj.attrib["type"], 16)
                _classType = obj.find('Class')

                if _classType == None or not any([x in _classType.text for x in filters]):
                    continue

                # we have this item already
                if not is_string_id(_id):
                    continue

                tries = 0
                while not is_valid_id(_type) and tries < 100:
                    _type += 1
                    tries += 1
                if tries == 100:
                    continue
                obj.attrib["type"] = hex(_type)

                open("./missingres/output.xml", 'a').write(ET.tostring(obj, encoding='unicode'))

                # for text in obj.findall('Texture'):
                #     if indx := text.find('Index') != None:
                #         hex_of_index = int(indx.text, 16)
                #         get_image_section(file, hex_of_index).save("image_rip/" + file.png)

path_to_port = "C:\\Users\\mpp43\\Documents\\GitHub\\ut-v4-source-master\\ut-core-master\\client\\src\\kabam\\rotmg\\assets"
# port_some_xml(path_to_port, filters=["Projectile"])
port_some_images(path_to_port, "./missingres/output.xml")
