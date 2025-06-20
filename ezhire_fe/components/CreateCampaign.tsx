import { CampaignPriority, client } from "@/api/client";
import { FormEvent, useState } from "react";

interface Props {
	open: boolean;
	setOpen: (state: boolean) => void;
	done: () => Promise<void>;
}

export function CreateCampaign({ open, setOpen, done }: Props) {
	const [name, setName] = useState("");
	const [priority, setPriority] = useState(CampaignPriority.LOW);
	const [error, setError] = useState("");

	async function saveCampaign(e: FormEvent) {
		e.preventDefault();

		const { error } = await client.POST("/api/campaigns", { body: { name, priority } });

		if (error) {
			setError("Nazwa jest wymagana");
			return;
		}

		setOpen(false);
		await done();
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
						<legend className="fieldset-legend">Utwórz Kampanię</legend>

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
						{error && <p className="label text-error">{error}</p>}

						<label htmlFor="priority" className="label">
							Priorytet
						</label>
						<select
							id="priority"
							defaultValue={CampaignPriority.LOW}
							className="select"
							onChange={(e) => setPriority(parseInt(e.target.value))}
						>
							<option value={CampaignPriority.LOW}>Niski</option>
							<option value={CampaignPriority.MEDIUM}>Średni</option>
							<option value={CampaignPriority.HIGH}>Wysoki</option>
						</select>
						<button className="btn btn-neutral">Utwórz</button>
					</fieldset>
				</form>
			</div>
		</div>
	);
}
