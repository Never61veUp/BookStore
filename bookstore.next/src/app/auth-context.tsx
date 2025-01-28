"use client"
import React, {createContext, useContext, useState, ReactNode, useEffect} from "react";
import Cookies from "js-cookie";
import { signIn, signOut } from "@/app/services/user";
import {notifyError, notifySuccess} from "@/app/layout";
import {useRouter} from "next/navigation";  // предполагается, что у вас есть эти функции

interface AuthContextType {
    loggedIn: boolean;
    login: (email: string, password: string) => Promise<void>;
    logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [loggedIn, setLoggedIn] = useState<boolean>(false);
    // const [roles, setRoles] = useState<string[]>([]);
    const router = useRouter();

    useEffect(() => {
        const token = Cookies.get("tasty-cookies");
        if (token) {
            setLoggedIn(true);
            // const decodedToken = JSON.parse(atob(token.split(".")[1]));
            // setRoles(decodedToken.roles || []);
        }
    }, []);

    const login = async (email: string, password: string) => {
        try {
            await signIn(email, password);
            notifySuccess("Успешная авторизация");
            router.push("/catalog");
            setLoggedIn(true);
        } catch (err) {
            if (err instanceof Error) {
                notifyError(err.message);
            }
        }
    };

    const logout = async () => {
        try {
            await signOut();
            setLoggedIn(false);
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <AuthContext.Provider value={{ loggedIn, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};