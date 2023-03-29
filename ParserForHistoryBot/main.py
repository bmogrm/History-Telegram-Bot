import array
import json
from email import message

import requests
from bs4 import BeautifulSoup

count = -1
new_json = []
pre_json = ""

url = "https://histerl.ru/dati#vi-ix-veka"
response = requests.get(url)
soup = BeautifulSoup(response.text, 'lxml')
quotes = soup.find_all("td")

for quote in quotes:
    if count % 2 == 0:
        new_json.append(pre_json + " --> " + quote.text)
    else:
        pre_json =quote.text
    count += 1

dates = json.dumps(new_json, ensure_ascii=False)


with open(r'C:\Rodion\testdate4.json', 'w', encoding='utf-8') as fh:
   fh.write(dates)