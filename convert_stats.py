stat_map = {
    "1": "0",
    "3": "1",
    "21": "3",
    "22": "4",
    "20": "2",
    "26": "6",
    "27": "7",
    "28": "5"
}

def do_file(file):
    contents=""
    with open(file, 'r') as f:
        for line in f:
            for statk in stat_map.keys():
                line = line.replace(f"stat=\"{statk}\"", f"stat=\"{stat_map[statk]}\"")
            contents += line
    with open(file, 'w') as f:
        f.write(contents)
