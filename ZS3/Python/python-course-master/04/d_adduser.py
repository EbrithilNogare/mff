#!/usr/bin/env python3

passwd = "./passwd"


def uid_exists(uid):
    ffh = open(passwd, 'r')
    for l in ffh:
        if str(uid) in l:
            return True

    return False


def find_uid():
    current = 1000
    while uid_exists(current):
        current += 1
    return(current)


user = input("User: ")
# Check if user exists
fh = open(passwd, 'r')
uid = find_uid()
print(f"UID is going to be {uid}. Is it ok?")
uid_ret = input("Y/N: ")
if uid_ret == "N":
    uid = input("Enter UID: ")

while uid_exists(int(uid)):
    uid = input("UID already exists. Enter UID: ")

print(f"UID: {uid}")
fh.close()
