"use client"
import {Card, CardContent, CardFooter, CardHeader, CardTitle} from "@/components/ui/card";
import {Button} from "@/components/ui/button";
import {Separator} from "@/components/ui/separator";
import {Input} from "@/components/ui/input";
import React from "react";
import {useState} from "react";
import {signIn, signUp, SignUpRequest} from "@/app/services/user";
import Link from "next/link";
import {notifyError, notifySuccess} from "@/app/layout";
import {useRouter} from "next/navigation";
import {AuthProvider, useAuth} from "@/app/auth-context";

export interface Props {
    mode: Mode;
}
export enum Mode {
    SignIn,
    SignUp,
}

export const AuthForm = ({mode}: Props) => {

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [firsName, setFirsName] = useState("");
    const [lastName, setLastName] = useState("");
    const [middleName, setMiddleName] = useState("");

    const router = useRouter();
    const {login} = useAuth();
    const handleOnOk = async () => {
        mode === Mode.SignIn
            ? await handleSignIn(email, password)
            : await handleSignUp({
                firstName: firsName,
                lastName: lastName,
                email: email,
                password: password,
                middleName: middleName,
            });
    };
    const handleSignUp = async (request: SignUpRequest) => {
        await signUp(request);
    }
    const handleSignIn = async(email: string, password: string) => {
        try {
            await login(email, password);
            notifySuccess("Успешная авторизация");
            router.push("/catalog");
        } catch (err) {
            if (err instanceof Error) {
                notifyError(err.message);
            }
        }
    }

    return (

        <div className="flex items-center justify-center min-h-screen bg-gray-50">
            <Card className="w-[350px]">
                <CardHeader>
                    <CardTitle>{mode === Mode.SignIn ? "Sign in to your account"
                        :"Create an account"}</CardTitle>
                    <p className="text-sm text-muted-foreground">
                        {mode === Mode.SignIn ? "Enter your email and password to continue"
                        : "Enter your email below to create your account"}
                    </p>
                </CardHeader>
                <CardContent className="space-y-4">
                    {mode === Mode.SignUp ? (
                        <div>
                            <Input onChange={(e) => setFirsName(e.target.value)} type="text"
                                   value={firsName} placeholder="Имя" required={true} />
                            <Input onChange={(e) => setLastName(e.target.value)} type="text"
                                   value={lastName} placeholder="Фамилия" required={true}/>
                            <Input onChange={(e) => setMiddleName(e.target.value)} type="text"
                                   value={middleName} placeholder="Отчество" required={false}/>
                        </div>

                    ) : (
                        <div>
                            <Button variant="outline" className="w-full">
                                <svg className="h-4 w-4 mr-2" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                                    <path
                                        d="M12 0L15.09 8.26H23.6L17.26 13.26L20.35 21.52L12 16.52L3.65 21.52L6.74 13.26L0.4 8.26H8.91L12 0Z"
                                        fill="currentColor"/>
                                </svg>
                                GitHub
                            </Button>
                            <Button variant="outline" className="w-full">
                                <svg className="h-4 w-4 mr-2" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                                    <path
                                        d="M21.35 11.1H12V13.05H17.54C16.83 15.5 14.68 16.5 12 16.5C9.27 16.5 6.69 14.48 6.08 11.9C5.48 9.32 7.25 6.77 9.96 6.13C12.36 5.57 14.99 7.34 15.29 9.83H17.51C16.99 6.71 14.19 3.9 10.93 3.39C6.94 2.65 3.5 6.05 3.5 10C3.5 13.95 6.96 17.35 10.96 16.6C14.19 16.08 16.99 13.26 17.51 10.13H21.35V11.1Z"
                                        fill="currentColor"/>
                                </svg>
                                Google
                            </Button>
                        </div>
                    )
                    }
                    <Separator>OR CONTINUE WITH</Separator>
                    <Input onChange={(e) => setEmail(e.target.value)} type="email" value={email} placeholder="Email"/>
                    <Input onChange={(e) => setPassword(e.target.value)} type="password" value={password}
                           placeholder="Password"/>
                </CardContent>
                <CardFooter>
                    <Button onClick={handleOnOk} className="w-full">{mode === Mode.SignIn ? "Sign in"
                        : "Create Account"}
                    </Button>
                    {mode === Mode.SignIn ? (
                        <p className="text-sm text-center text-muted-foreground">
                            Don&#39;t have an account?{" "}
                            <Link href="/SignUp" className="text-blue-500 hover:underline">
                                Sign up
                            </Link>
                        </p>
                    ) : (
                        <p className="text-sm text-center text-muted-foreground">
                            Уже зарегистрированы?{" "}
                            <Link href="/SignIn" className="text-blue-500 hover:underline">
                                Войти
                            </Link>
                        </p>
                    )}
                </CardFooter>
            </Card>
        </div>
    )
}