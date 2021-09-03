import React, { useEffect, useState } from "react";
//load service for conect with Api
import service from "./service/index";
import Movie from "./Components/Movie/Movie";
import "./App.css";

function App() {
	const api = new service();

	const [movies, setMovies] = useState([]);
	const [search, setSearch] = useState("");
	const [forNameMovie, setForNameMovie] = useState(false);
	const [forGenreMovie, setForGenreMovie] = useState(false);
	const [forNameActor, setForNameActor] = useState(false);

	const loadMovies = async () => {
		try {
			api.setEndPoint("movie");
			let searchTerm = search;
			let searchProperty = "";

			let dataSearch = {
				currentPage: 0,
				pageSize: 10,
				searchTerm,
				searchProperty,
			};

			//Create concatenated search necesari for query
			if (forNameMovie) {
				dataSearch.searchProperty = "Name";
			}
			if (forGenreMovie) {
				if (dataSearch.searchProperty.length > 0)
					dataSearch.searchProperty += "-";
				dataSearch.searchProperty += "Genre";
			}

			//for dificult process, is created new only EndPoint for search
			if (forNameActor) {
				api.setEndPoint("movie/SearchWithActorName");
				if (dataSearch.searchProperty.length > 0)
					dataSearch.searchProperty += "-";
				dataSearch.searchProperty += "Actor.Name";
			}

			if (dataSearch.searchProperty === "") {
				setForNameMovie(true);
				dataSearch.searchProperty += "Name";
			}

			let json = await api.getAll(dataSearch);

			setMovies(json.data.data);
		} catch (error) {
			console.log(error);
		}
	};

	useEffect(() => {
		(async () => {
			await loadMovies();
		})();
	}, []);

	return (
		<div className="App">
			<div className="header">
				<div className="search">
					<input
						type="text"
						className="text"
						value={search}
						onChange={(e) => setSearch(e.target.value)}
					/>
					<input
						type="button"
						className="button"
						value="search"
						onClick={loadMovies}
					/>
				</div>
				<div className="check__box">
					<label>
						<input
							key="0"
							type="checkbox"
							checked={forNameMovie}
							onClick={() => setForNameMovie(!forNameMovie)}
						/>
						{"Name movie"}
					</label>

					<label>
						<input
							key="1"
							id="gm"
							type="checkbox"
							checked={forGenreMovie}
							onClick={() => setForGenreMovie(!forGenreMovie)}
						/>
						{"Genre movie"}
					</label>

					<label>
						<input
							key="2"
							id="na"
							type="checkbox"
							checked={forNameActor}
							onClick={() => setForNameActor(!forNameActor)}
						/>
						{"Name actor"}
					</label>
				</div>
			</div>
			<div className="movies__container">
				{movies.map((movie, index) => (
					<Movie key={index} value={movie} />
				))}
			</div>
		</div>
	);
}

export default App;
