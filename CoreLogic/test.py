import time

def print_loading_bar(percentage):
	total_blocks = 10
	filled_blocks = int(percentage / 10)
	empty_blocks = total_blocks - filled_blocks
	
	bar = "█" * filled_blocks + "░" * empty_blocks
	print(f"\rLoading: [{bar}] {percentage}%", end="")

# Quick test run
for i in range(0, 101, 10):
	print_loading_bar(i)
	time.sleep(0.2)