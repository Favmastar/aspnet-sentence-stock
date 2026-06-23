import json
import urllib.request


ENDPOINT = "http://localhost:5000/graphql/"


SAMPLES = [
    {
        "text": "Small, complete slices make progress visible.",
        "language": "English",
        "source": "Seed",
        "notes": "Good for project retrospectives.",
        "tags": ["planning", "portfolio"],
    },
    {
        "text": "例文は文脈と一緒に保存すると再利用しやすい。",
        "language": "Japanese",
        "source": "Seed",
        "notes": "Reminder for language learning.",
        "tags": ["learning", "language"],
    },
]


def graphql(query: str, variables: dict) -> dict:
    body = json.dumps({"query": query, "variables": variables}).encode("utf-8")
    request = urllib.request.Request(
        ENDPOINT,
        data=body,
        headers={"Content-Type": "application/json"},
        method="POST",
    )

    with urllib.request.urlopen(request, timeout=10) as response:
        return json.loads(response.read().decode("utf-8"))


def main() -> None:
    mutation = """
    mutation CreateSentence($input: SentenceInput!) {
      createSentence(input: $input) {
        id
        text
      }
    }
    """

    for sample in SAMPLES:
        result = graphql(mutation, {"input": sample})
        created = result["data"]["createSentence"]
        print(f"created #{created['id']}: {created['text']}")


if __name__ == "__main__":
    main()
