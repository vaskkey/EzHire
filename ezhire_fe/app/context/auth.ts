import { User } from "@/api/client";
import { createContext } from "react";

export const UserContext = createContext<User | null>(null);
