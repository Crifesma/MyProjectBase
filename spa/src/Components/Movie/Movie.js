import React from "react";
import "./Movie.css";

function Movie(props) {
	return (
		<div
			className="movie"
			style={{ backgroundImage: `url(${props.value.urlImg})` }}
		>
			<div className="id">{props.value.id}</div>
			<h2 className="movie__title">{props.value.name}</h2>
			<div className="movie__infos">
				<div className="movie__cast">Cast</div>
				<div className="movie__cast">
					{props.value.actors?.map((actor, index) => (
						<div key={index}> {actor.name} </div>
					))}
				</div>

				<div className="movie__genre">Genre</div>
				<div className="movie__genre">{props.value.genre}</div>
			</div>
		</div>
	);
}

export default Movie;
