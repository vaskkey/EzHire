"use client";

import { client } from "@/api/client";
import { redirect } from "next/navigation";
import { FormEvent, useState } from "react";

export function LoginForm() {
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [error, setError] = useState("");

	async function login(e: FormEvent) {
		e.preventDefault();

		const { error, response } = await client.POST("/api/auth/login", {
			body: { email, password },
		});

		if (error || response.status !== 200) {
			setError("Nie udało się zalogować");
			return;
		}

		redirect("/campaigns");
	}

	return (
		<div>
			<h2 className="font-extrabold text-3xl mb-4 text-center">EzHire</h2>
			<form onSubmit={login}>
				<fieldset className="fieldset bg-background border-base-300 rounded-box w-xs border p-4">
					<legend className="fieldset-legend">Zaloguj się</legend>

					<label className="label">Email</label>
					<input
						type="email"
						className="input"
						placeholder="Email"
						value={email}
						onChange={(ev) => setEmail(ev.target.value)}
					/>
					{error && <p className="label text-error">{error}</p>}

					<label className="label">Hasło</label>
					<input
						type="password"
						className="input"
						placeholder="Hasło"
						value={password}
						onChange={(ev) => setPassword(ev.target.value)}
					/>

					<button className="btn btn-neutral mt-4">Login</button>
				</fieldset>
			</form>
		</div>
	);
}
