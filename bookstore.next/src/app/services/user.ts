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
    await fetch("http://localhost:5263/User/api/user/register", {
        method: "POST",
        body: JSON.stringify(data),
        headers: {
            "Content-Type": "application/json",
        },
    });
};