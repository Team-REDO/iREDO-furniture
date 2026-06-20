import { Button } from "@/components/ui/button";
import { Card, CardAction, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";

import { API_BASE_URL } from "@/config.ts";

export function LoginCard() {
  return (
    <>
      <Card className="w-full max-w-sm">
        <CardHeader>
          <CardTitle>Login to your account</CardTitle>
          <CardAction>
            <Button variant="link">Sign Up</Button>
          </CardAction>
        </CardHeader>
        <CardFooter className="flex-col gap-2">
          <Button
            variant="outline"
            className="w-full"
            onClick={() => {
              window.location.href = `${API_BASE_URL}/api/auth/google-login`;
            }}
          >
            Login with Google
          </Button>
        </CardFooter>
      </Card>
    </>
  );
}
