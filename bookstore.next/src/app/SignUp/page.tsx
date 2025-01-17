import {AuthForm, Mode} from "@/components/shared/authForm";

export default function Home() {
    return (
        <>
            <AuthForm mode={Mode.SignUp}/>
        </>
    );
}
