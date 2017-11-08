<?php
	//echo "Hello World";
	
	$host = "localhost";
	$database="playerlogintimes";
	$user="root";
	$password="";
	$error="Can't connect to database";
	$con=mysqli_connect($host,$user,$password);
	
	mysqli_select_db($con, $database) or die("Unable to connect to database");
	
	$name = $_GET['name'];
	$score = $_GET['score'];
	echo "name: ".$name."\n";
	
	$query = "SELECT * FROM scores WHERE name = '$name'";
	$result = mysqli_query($con, $query);
	$n = mysqli_num_rows($result);
	$date = date("Y/m/d");
	
	if($n > 0){
		echo "found one record";
		if($score > mysqli_fetch_assoc($result)["score"]){
		
			$query = "UPDATE scores SET score = '$score' WHERE name = '$name'";
		}
	} 
	else {
		echo $name." not registered";
		$query = "INSERT INTO scores VALUES ('$name','$score','$date')";
	}
	//echo $date;
	$result = mysqli_query($con, $query);
?>

