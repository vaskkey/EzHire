import { client } from "@/api/client";
import { FormEvent, useEffect, useState } from "react";

interface Props {
	open: boolean;
	campaignId: number;
	setOpen: (state: boolean) => void;
	done: () => Promise<void>;
}

interface Errors {
	JobName?: string[];
	Description?: string[];
}

export function CreatePosting({ open, setOpen, done, campaignId: postingId }: Props) {
	const [name, setName] = useState("");
	const [description, setDescription] = useState("");

	const [nameError, setNameError] = useState("");
	const [descError, setDescError] = useState("");

	useEffect(() => {
		if (!open) {
			setName("");
			setDescription("");
			setNameError("");
			setDescError("");
		}
	}, [open]);

	async function saveCampaign(e: FormEvent) {
		e.preventDefault();

		setNameError("");
		setDescError("");

		const { error } = await client.POST("/api/campaigns/{id}/create-posting", {
			params: { path: { id: postingId } },
			body: { jobName: name, description },
		});

		if (error && "errors" in error) {
			handleErrors(error.errors as Errors);
			return;
		}

		setOpen(false);
		await done();
	}

	function handleErrors(errs: Errors) {
		if (errs.JobName) {
			setNameError("Nazwa jest wymagana.");
		}

		if (errs.Description) {
			setDescError("Opis jest wymagany.");
		}
	}

	return (
		<div className="drawer drawer-end">
			<input
				id="campaign-open"
				type="checkbox"
				className="drawer-toggle"
				checked={open}
				onChange={(e) => setOpen(e.target.value === "true")}
			/>
			<div className="drawer-side">
				<label htmlFor="campaign-open" aria-label="close sidebar" className="drawer-overlay"></label>
				<form onSubmit={saveCampaign} className="menu bg-base-200 text-base-content min-h-full w-96 p-4">
					<fieldset className="fieldset p-4">
						<legend className="fieldset-legend">Utwórz Ogłoszenie</legend>

						<label htmlFor="title" className="label">
							Nazwa
						</label>
						<input
							id="title"
							type="text"
							className="input"
							placeholder="Moja Kampania"
							value={name}
							onChange={(e) => setName(e.target.value)}
						/>
						{nameError && <p className="label text-error">{nameError}</p>}

						<label htmlFor="description" className="label">
							Opis
						</label>
						<textarea
							id="description"
							className="input"
							placeholder="Mój opis"
							value={description}
							onChange={(e) => setDescription(e.target.value)}
						/>
						{descError && <p className="label text-error">{descError}</p>}
						<button className="btn btn-success">Utwórz</button>
					</fieldset>
				</form>
			</div>
		</div>
	);
}
