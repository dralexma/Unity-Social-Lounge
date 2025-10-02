import os
from PIL import Image

# Goal: square to 737 * 735, paste frame, finally save to 1024 * 1024

def centerEndpoints(oriLength, cutLength):
	diff = (oriLength - cutLength) // 2
	return (diff, oriLength-diff)

def toSquare(img, pixels):

	width, height = img.size
	minlen = min(width, height)

	w1, w2 = centerEndpoints(width, minlen)
	h1, h2 = centerEndpoints(height, minlen)

	cropped = img.crop((w1,h1,w2,h2))
	resized = cropped.resize(pixels)

	return resized

def createFrame(img, pixels):

	white = Image.new("RGB", pixels, (255,255,255))
	frame = img.convert("RGB").copy()
	frame.paste(white)
	frame.save('frame.png')

	return frame

def makeFramed(img, pixels, frame):

	squared = toSquare(img, pixels)
	result = frame.copy()
	result.paste(squared)
	# convert to palettised mode for smaller storage
	result.convert('P')

	return result

if __name__ == '__main__':

	fixed_size = (737,735)

	raw_dir = "../Raw Images"
	fixed_dir = "../Fixed Images"

	frame = Image.open("frame.png")

	for file in os.listdir(raw_dir):
		if file.endswith((".jpg",".JPG","jpeg","JPEG",".png",".PNG")):
			img = Image.open(f"{raw_dir}/{file}")
			fn, fext = os.path.splitext(file)

			result = makeFramed(img, fixed_size, frame)
			result.save(f'{fixed_dir}/{fn}.png')



