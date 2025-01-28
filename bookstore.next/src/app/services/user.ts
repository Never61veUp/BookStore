export interface SignUpRequest {
    firstName: string;
    lastName: string;
    middleName?: string;
    email: string;
    password: string;
}
export interface SignInRequest {
    email: string;
    password: string;
}

export const signUp = async (data: SignUpRequest) => {
    console.log("Request Data:", data);
    await fetch("http://localhost:5263/api/user/signUp", {
        method: "POST",
        body: JSON.stringify(data),
        headers: {
            "Content-Type": "application/json",
        },
    });
};

export async function signIn(email: string, password: string) {
    const response = await fetch('http://localhost:5263/api/user/signIn', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
        credentials: 'include',
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || 'Ошибка авторизации');
    }

    return await response.text();
}

export async function signOut() {
    const response = await fetch('http://localhost:5263/api/user/signOut', {
        method: 'POST',
        credentials: 'include', // Чтобы учитывать куки
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || 'Ошибка выхода');
    }
}
export async function isLoggedIn(): Promise<boolean> {
    const response = await fetch('http://localhost:5263/api/user/isLoggedIn', {
        method: 'GET',
        credentials: 'include',
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || 'Ошибка итендификации');
    }
    const result = await response.text();
    return result.trim().toLowerCase() === 'true';
}