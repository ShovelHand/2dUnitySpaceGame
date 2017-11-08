<?php
	//echo "Hello World";
	
	$host = "localhost";
	$database="playerlogintimes";
	$user="root";
	$password="";
	$error="Can't connect to database";
	$con=mysqli_connect($host,$user,$password);
	
	mysqli_select_db($con, $database) or die("Unable to connect to database");
	
	$query = "SELECT * FROM scores";
	
	$result = mysqli_query($con, $query);
	
	$n = mysqli_num_rows($result);
	
	for($i = 0; $i < $n; $i++)
	{
			
		$name  = mysqli_fetch_assoc($result)["name"];
		$score = mysqli_fetch_assoc($result)["score"];
		$date = strtotime(mysqli_fetch_assoc($result)["date"]);
		
		echo "Name: ".$name."\t";
		echo "Score: ".$score."\t";
		echo "Date: ".$date."\n";
		
	}
	
?>

