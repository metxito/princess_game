import numpy as np
import cv2
import os

tilewidth = 5
alpha_limit = 0.3



def merge_tiles(path, result_path):
  imagelst = []
  size_x = -1.0
  size_y = -1.0
  size_ch = -1
  os.chdir(".")
  for file in os.listdir(path):
    img = cv2.imread(path + file, cv2.IMREAD_UNCHANGED) 
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
  print("For " + result_path + " there are a total of " + str(total) + " on " + path)
  print("with size: (" + str(size_x) + ", " + str(size_y) + ") and " + str(size_ch) + " channels")
  new_y = tilewidth
  new_x = int(total / tilewidth)
  if (total % tilewidth) > 0:
    new_x += 1

  new_x = new_x * size_x
  new_y = new_y * size_y
  new_ch = size_ch

  result = np.zeros((new_x, new_y, new_ch), np.uint8)
  pos_x = 0
  pos_y = 0
  pos_xw = pos_x + size_x
  pos_yh = pos_y + size_y

  for img in imagelst:
    result[pos_x:pos_xw, pos_y:pos_yh, :4] = img
    pos_y = pos_y + size_y
    if pos_y >= new_y:
      pos_x = pos_x + size_x
      pos_y = 0
    pos_xw = pos_x + size_x
    pos_yh = pos_y + size_y

  for x in range(0, new_x - 1):
    for y in range(0, new_y - 1):
      if result[x, y, 3] > alpha_limit * 255:
        result[x, y, 3] = 255
      else:
        result[x, y, 3] = 0

  cv2.imwrite(result_path, result )
  


merge_tiles ("test/", "test_result.png")
merge_tiles ("caballero/tiles_caminando/", "caballero/caballero_walking.png")
merge_tiles ("caballero/tiles_salto/", "caballero/caballero_jumping.png")
merge_tiles ("caballero/tiles_idle_1/", "caballero/caballero_idle_1.png")