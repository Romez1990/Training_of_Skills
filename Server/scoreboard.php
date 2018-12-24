<?php

$conn = mysqli_connect('127.0.0.1', 'root', '', 'training_of_skills');

$name = $_POST['name'] !== null ? $_POST['name'] : $_GET['name'];
$score = $_POST['score'] !== null ? $_POST['score'] : $_GET['score'];

$added = strlen($name) >= 4 && intval($score) > 0;

if ($added) {
	mysqli_query($conn, "INSERT INTO `scoreboard` (`name`, `score`) VALUE ('$name', '$score')");
	$id = mysqli_insert_id($conn);
}

function record_format(&$record) {
	unset($record['id']);
	$record['datetime'] = date("m/d/Y H:i", strtotime($record['datetime']));
	return $record;
}

mysqli_query($conn, "SET @i = 0");
$top_records_db = mysqli_query($conn, "SELECT (@i := @i + 1) AS 'position', `id`, `name`, `score`, `datetime` FROM `scoreboard` ORDER BY `score` DESC, `datetime` DESC LIMIT 10");

$current_found = false;
$top_records = null;
while ($record = mysqli_fetch_assoc($top_records_db)) {
	if (intval($record['id']) === $id)
		$current_found = true;
	
	$top_records[] = record_format($record);
}

if ($added) {
	for ($start_range = 10, $end_range = $start_range + 100; !$current_found; $start_range += 100, $end_range += 100) {
		$remaining_records = mysqli_query($conn, "SELECT (@i := @i + 1) AS 'position', `id`, `name`, `score`, `datetime` FROM `scoreboard` WHERE `score` >= 0 ORDER BY `score` DESC, `datetime` DESC LIMIT " . $start_range . ", " . $end_range);
		while (($record = mysqli_fetch_assoc($remaining_records)) && !$current_found) {
			if (intval($record['id']) !== $id) continue;
			
			$current_found = true;
			$top_records[9] = record_format($record);
		}
	}
}

echo json_encode($top_records);
