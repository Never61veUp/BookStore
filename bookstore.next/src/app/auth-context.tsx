"use client"
import React, {createContext, useContext, useState, ReactNode, useEffect} from "react";
import Cookies from "js-cookie";
import { signIn, signOut } from "@/app/services/user";  // предполагается, что у вас есть эти функции

interface AuthContextType {
    loggedIn: boolean;
    login: (email: string, password: string) => Promise<void>;
    logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [loggedIn, setLoggedIn] = useState<boolean>(false);
    useEffect(() => {
        setLoggedIn(!!Cookies.get('tasty-cookies'));
    }, []);

    const login = async (email: string, password: string) => {
        try {
            await signIn(email, password);
            setLoggedIn(true);
        } catch (err) {
            console.error(err);
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