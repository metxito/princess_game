import cv2
import os
import numpy as np

tilewidth = 5
alpha_limit = 0.3
path ="test"


imagelst = []
size_x = -1.0
size_y = -1.0
size_ch = -1

os.chdir(".")
for file in os.listdir("test/"):
  print(file)
  img = cv2.imread("test/" + file, cv2.IMREAD_UNCHANGED) 
  imagelst.append(img)
  if size_x < 0 or size_y < 0 or size_ch < 0:
    size_x = img.shape[0]
    size_y = img.shape[1]
    size_ch = img.shape[2]
  else:
    if size_x != img.shape[0]:
      print('error')
    if size_y != img.shape[1]:
      print('error')
    if size_ch != img.shape[2]:
      print('error')

total = len(imagelst)
print(total)

new_y = tilewidth
new_x = int(total / tilewidth)
if (total % tilewidth) > 0:
  new_x += 1

new_x = new_x * size_x
new_y = new_y * size_y
new_ch = size_ch

print (new_x)
print (new_y)


result = np.zeros((new_x, new_y, new_ch), np.uint8)
pos_x = 0
pos_y = 0
pos_xw = pos_x + size_x
pos_yh = pos_y + size_y



for img in imagelst:
  print(str(pos_x) + ", " + str(pos_y) + "  >>  " + str(pos_xw) + ", " + str(pos_yh))
  result[pos_x:pos_xw, pos_y:pos_yh, :4] = img

  pos_y = pos_y + size_y
  if pos_y >= new_y:
    pos_x = pos_x + size_x
    pos_y = 0
  
  pos_xw = pos_x + size_x
  pos_yh = pos_y + size_y







#result = None
#for img in imagelst:
#  if result is None:
#    result = img
#  else:
#    result = np.concatenate((result, img), axis=1)

for x in range(0, new_x - 1):
  for y in range(0, new_y - 1):
    if result[x, y, 3] > alpha_limit * 255:
      result[x, y, 3] = 255
    else:
      result[x, y, 3] = 0



cv2.imwrite("result.png", result )
  